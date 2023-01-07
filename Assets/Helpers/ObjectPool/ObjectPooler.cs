using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Helpers.ObjectPool
{
     /*
  *    Unit Object Pooler Implemenation Created By Jared Massa
  *
  *    Copyright (c) 2020 Jared Massa
  *
  *    Permission is hereby granted, free of charge, to any person obtaining a copy
  *    of this software and associated documentation files (the "Software"), to deal
  *    in the Software without restriction, including without limitation the rights
  *    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
  *    copies of the Software, and to permit persons to whom the Software is
  *    furnished to do so, subject to the following conditions:
  *
  *    The above copyright notice and this permission notice shall be included in all
  *    copies or substantial portions of the Software.
  *
  *    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
  *    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
  *    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
  *    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
  *    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
  *    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
  *    SOFTWARE.
  */


    #region PoolableType Definition

    [System.Serializable]
    public class PooledType
    {
        public string TypeName;

        public GameObject Prefab;

        public string SortingTag = "";

        public int Max;
        public bool AutoExpand;
    }

    #endregion

    public class ObjectPooler : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private bool UseParentTransforms = false;

        // private variables
        private Dictionary<string, PooledType> _tagTypeLookup;
        private Dictionary<GameObject, PooledType> _prefabTypeLookup;
        private Dictionary<string, Queue<GameObject>> _sleepingObjects;
        private Dictionary<string, HashSet<GameObject>> _activeObjects;
        private Dictionary<string, Transform> _typeParents;

        #endregion

        #region Initialize

        public void Initialize(List<PooledType> poolableTypes)
        {
            // init dictionaries
            if (UseParentTransforms)
                _typeParents = new Dictionary<string, Transform>();
            _tagTypeLookup = new Dictionary<string, PooledType>();
            _prefabTypeLookup = new Dictionary<GameObject, PooledType>();
            _sleepingObjects = new Dictionary<string, Queue<GameObject>>();
            _activeObjects = new Dictionary<string, HashSet<GameObject>>();
            // init types and sets in dicts
            foreach (PooledType type in poolableTypes)
            {
                if (UseParentTransforms)
                {
                    GameObject par = new GameObject(type.TypeName + " Parent");
                    par.transform.parent = transform;
                    _typeParents[type.SortingTag] = par.transform;
                }

                // add reference to type
                _tagTypeLookup.Add(type.SortingTag, type);
                _prefabTypeLookup.Add(type.Prefab, type);
                _sleepingObjects.Add(type.SortingTag, new Queue<GameObject>());
                _activeObjects.Add(type.SortingTag, new HashSet<GameObject>());
                // init sleeping objects
                for (int i = 0; i < type.Max; i++)
                {
                    GameObject t = Instantiate(type.Prefab,
                        UseParentTransforms ? _typeParents[type.SortingTag] : transform);
                    t.tag = type.SortingTag;
                    t.SetActive(false);
                    _sleepingObjects[type.SortingTag].Enqueue(t);
                }
            }
        }

        #endregion

        #region Destroy

        public void Destroy(GameObject obj) => _Destroy(obj);

        /// <summary>
        /// Deactivate an active object, and enqueue it for later restoration.
        /// </summary>
        /// <param name="obj">The active object to deactivate</param>
        private void _Destroy(GameObject obj)
        {
            // check if this object is one we actually pool
            if (!_activeObjects.ContainsKey(obj.tag)) return;
            // check if this object is actually in our active set right now
            if (!_activeObjects[obj.tag].Contains(obj)) return;
            // since it is, we will deactivate the object
            obj.SetActive(false);
            // then add it to the sleeping queue
            _sleepingObjects[obj.tag].Enqueue(obj);
            // and remove it from the active set
            _activeObjects[obj.tag].Remove(obj);
        }

        #endregion

        #region Generate

        /// <summary>
        /// Grab a member from the list of SleepingObjects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier">Either the Prefab to instantiate or the Tag for the PoolableType</param>
        /// <param name="position">The Position to activate the object at</param>
        /// <param name="rotation">The Rotation to activate the object with</param>
        /// <returns>A gameobject from the correct pool</returns>
        public GameObject Generate<T>(T identifier, Vector3 position, Quaternion rotation) =>
            _Generate(identifier, position, rotation);

        /// <summary>
        /// Grab a member from the list of SleepingObjects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier">Either the Prefab to instantiate or the Tag for the PoolableType</param>
        /// <param name="position">The Position to activate the object at</param>
        /// <returns>A gameobject from the correct pool</returns>
        public GameObject Generate<T>(T identifier, Vector3 position) =>
            _Generate(identifier, position, Quaternion.identity);

        /// <summary>
        /// Grab a member from the list of SleepingObjects
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="identifier">Either the Prefab to instantiate or the Tag for the PoolableType</param>
        /// <returns>A gameobject from the correct pool</returns>
        public GameObject Generate<T>(T identifier) =>
            _Generate(identifier, Vector3.zero, Quaternion.identity);

        private GameObject _Generate<T>(T identifier, Vector3 position, Quaternion rotation)
        {
            // get the tag of the prefab we wish to instantiate
            string tag = "";
            // user passed us a tag
            if (identifier is string)
            {
                if (!_sleepingObjects.ContainsKey(identifier as string)) return null;
                else tag = identifier as string;
            }
            // user passed us a prefab
            else if (identifier is GameObject)
            {
                if (!_prefabTypeLookup.ContainsKey(identifier as GameObject)) return null;
                else tag = _prefabTypeLookup[identifier as GameObject].SortingTag;
            }
            else return null;

            // If we make it here, we know the tag has been set and exists.
            GameObject member = null;
            // if no objects are left in the queue, but we can auto expand, then make a new object
            if (_sleepingObjects[tag].Count == 0 && _tagTypeLookup[tag].AutoExpand)
                member = Instantiate(_tagTypeLookup[tag].Prefab, UseParentTransforms ? _typeParents[tag] : transform);
            // else if there are members in the queue get the member from the sleeping queue
            else if (_sleepingObjects[tag].Count > 0)
                member = _sleepingObjects[tag].Dequeue();
            // else we cannot instantiate
            else return null;

            // Now that we have a member, we can update it's variables and return it
            member.transform.position = position;
            member.transform.rotation = rotation;
            _activeObjects[tag].Add(member);
            // can't forget to do this!
            member.SetActive(true);

            return member;
        }

        #endregion
    }

    #region TagSelectorDefinition

#if UNITY_EDITOR
//Original by DYLAN ENGELMAN http://jupiterlighthousestudio.com/custom-inspectors-unity/
//Altered by Brecht Lecluyse http://www.brechtos.com
    public class TagSelectorAttribute : PropertyAttribute
    {
        public bool useDefaultTagFieldDrawer = false;
    }

    [CustomPropertyDrawer(typeof(TagSelectorAttribute))]
    public class TagSelectorPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);

                var attrib = attribute as TagSelectorAttribute;

                if (attrib.useDefaultTagFieldDrawer)
                    property.stringValue = EditorGUI.TagField(position, label, property.stringValue);

                else
                {
                    //generate the taglist + custom tags
                    List<string> tagList = new List<string>();
                    tagList.AddRange(UnityEditorInternal.InternalEditorUtility.tags);
                    string propertyString = property.stringValue;
                    int index = -1;
                    if (propertyString == "")
                    {
                        //The tag is empty
                        index = 0; //first index is the special <notag> entry
                    }
                    else
                    {
                        //check if there is an entry that matches the entry and get the index
                        //we skip index 0 as that is a special custom case
                        for (int i = 1; i < tagList.Count; i++)
                        {
                            if (tagList[i] == propertyString)
                            {
                                index = i;
                                break;
                            }
                        }
                    }

                    //Draw the popup box with the current selected index
                    index = EditorGUI.Popup(position, label.text, index, tagList.ToArray());

                    //Adjust the actual string value of the property based on the selection
                    if (index == 0)
                        property.stringValue = "";
                    else if (index >= 1)
                        property.stringValue = tagList[index];
                    else
                        property.stringValue = "";
                }

                EditorGUI.EndProperty();
            }
            else
                EditorGUI.PropertyField(position, property, label);
        }
    }
#endif

    #endregion
}