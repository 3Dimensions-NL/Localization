using System.Collections.Generic;
using System.Linq;
using _3Dimensions.Localization.Runtime.Scripts;
using _3Dimensions.Localization.Runtime.Scripts.Translations;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Tools.Editor.Scripts.CSV;
using UnityEditor;
using UnityEngine;
namespace _3Dimensions.Localization.Editor.Scripts
{
    public class LocalizationEditor : OdinEditorWindow
    {
        public enum TranslationType
        {
            String,
            Sprite,
            AudioClip
        }
        
        private static readonly string _settingsPath = "LocalizationSettings";
        
        [MenuItem("3Dimensions/Localization/Localization Editor")]
        private static void OpenWindow()
        {
            var window = GetWindow<LocalizationEditor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
            _settings = Resources.Load<LocalizationSettings>(_settingsPath);
            if (!System.IO.Directory.Exists(_settings.pathToStoreTranslations))
            {
                string newPath = EditorUtility.OpenFolderPanel("Path to save localizations",  Application.dataPath + "/Localizations", null);
                if (!string.IsNullOrEmpty(newPath)) _settings.pathToStoreTranslations = newPath;
            }
            
            LoadSettings();
        }
        
        [BoxGroup("Settings")] [Button("Reload Settings")]
        [PropertyOrder(-1)]
        private static void LoadSettings()
        {
            LoadTranslations();
        }
        
        [BoxGroup("Settings")] [Button]
        [PropertyOrder(-1)]
        private void SetTranslationsFolder()
        {
            LoadSettings();
            //Set path
            string newPath = EditorUtility.OpenFolderPanel("Path to save localizations", _settings.pathToStoreTranslations, null);
            if (!string.IsNullOrEmpty(newPath)) _settings.pathToStoreTranslations = newPath;
            LoadTranslations();
        }
        
        private static LocalizationSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Resources.Load<LocalizationSettings>(_settingsPath);
                }

                return _settings;
            }
        }

        [BoxGroup("Settings")] [ShowInInspector]
        [PropertyOrder(-1)]
        private static LocalizationSettings _settings;
        
        [BoxGroup("Import & Export")] [HorizontalGroup("Import & Export/Buttons")]
        [PropertyOrder(-1)]
        [Button]
        private void ImportCSV()
        {
            //Import CSV
            string[][] dataRows = CsvFileReaderAndWriter.GetCsvData(true);
            Debug.Log("Importing " + dataRows.Length + " CSV rows.");
            
            //Get header (translations)
            int itemCount = dataRows[0].Length;
            LanguageObject[] importedTranslations = new LanguageObject[itemCount-1];

            //Set used languages (first item is description)
            for (int item = 1; item < itemCount; item++)
            {
                foreach (LanguageObject language in Settings.defaultLanguageSet)
                {
                    if (language.name == dataRows[0][item])
                    {
                        importedTranslations[item - 1] = language;
                        Debug.Log("Used language found: " + language.name);
                    }
                }
            }
            
            //Create or overwrite scriptable objects (skip header row)
            for (int row = 1; row < dataRows.Length; row++)
            {
                string fileName = dataRows[row][0];
                // Debug.Log("Filename to create = " + fileName);
                CreateTranslationScriptableObject(fileName, TranslationType.String);
                
                //Find translation for writing text
                TranslationAsset translationAsset = _translationsList.Find(x => x.name == fileName);
                if (translationAsset == null)
                {
                    Debug.LogError("Could not find TranslationObject: " + fileName);
                    break;
                }

                if (translationAsset.GetType() == typeof(TranslationAssetString))
                {
                    TranslationAssetString translationAssetString = translationAsset as TranslationAssetString;

                    //Create translations array
                    translationAssetString.translations =
                        new TranslationAssetString.TranslationString[importedTranslations.Length];

                    Debug.Log("Writing translations to lastCreatedTranslation: " + translationAsset.name);
                    for (int item = 1 ; item < itemCount; item++)
                    {
                        Debug.Log("Translation = " + item + ", " + dataRows[row][item]);
                        translationAssetString.translations[item - 1].language = importedTranslations[item - 1];
                        if (translationAssetString.translations[item - 1] is TranslationAssetString.TranslationString ts) ts.text = dataRows[row][item];
                    }
                    
                    EditorUtility.SetDirty(translationAsset);
                }
            }
        }
        
        [BoxGroup("Import & Export")] [HorizontalGroup("Import & Export/Buttons")]
        [PropertyOrder(-1)]
        [Button]
        private void ExportCSV()
        {
            //Collect and store data as strings;
            //First row with headers
            string firstRow = "Translation Name";
            List<string> csvRows = new List<string>();
            
            List<string> languageList = new List<string>();
            
            //Add languages to header as columns
            foreach (LanguageObject language in Settings.defaultLanguageSet)
            {
                //Store used Languages
                languageList.Add(language.name);
                
                //Create header column
                firstRow += "," + language.name;
            }
            
            //Add first row to csv data rows
            csvRows.Add(firstRow);

            foreach (TranslationAsset translationObject in _translationsList)
            {
                
                if (translationObject.GetType() == typeof(TranslationAssetString))
                {
                    TranslationAssetString translationAssetString = translationObject as TranslationAssetString;

                    string translationRow = translationObject.name;

                    //Add translation by language column
                    foreach (string language in languageList)
                    {
                        foreach (TranslationAssetString.TranslationString translation in translationAssetString.translations)
                        {
                            if (translation.language.name == language)
                            {
                                string value = translation.GetValue<string>();
                                if (string.IsNullOrEmpty(value))
                                {
                                    translationRow = translationRow + ",\"" + value + "\"";
                                }
                            }
                        }
                    }
                
                    // Debug.Log("Translation row = " + translationRow);
                    csvRows.Add(translationRow);
                };
            }
            
            CsvFileReaderAndWriter.WriteCsv(csvRows.ToArray());
        }
        
        [BoxGroup("New Translation")] [ShowInInspector]
        public static string NewTranslationName;

        private static string PathToStoreTranslations => Settings.pathToStoreTranslations;
        private static string FullTranslationName => Settings.prefix + NewTranslationName;

        [BoxGroup("Translations")]
        [ShowInInspector]
        private static TranslationAsset LastCreatedTranslation;
        
        [BoxGroup("Translations")] [ShowInInspector, ListDrawerSettings(HideAddButton = true, DraggableItems = false, ShowFoldout = false, ShowIndexLabels = false, ShowPaging = false, ShowItemCount = true, HideRemoveButton = true)]
        private static List<TranslationAsset> _translationsList = new List<TranslationAsset>();

        public static void LoadTranslations()
        {
            //Lookup translations in path
            List<TranslationAsset> tempList = new(FindAllScriptableObjectsOfType<TranslationAsset>("t:TranslationAsset", PathToStoreTranslations));
            
            if (tempList.Count == 0)
            {
                _translationsList.Clear();
            }
            else
            {
                _translationsList = new List<TranslationAsset>(tempList);
            }
            
            try
            {
                GUIUtility.ExitGUI();
            }
            catch
            {
                // ignored
            }
        }

        [BoxGroup("New Translation"), HorizontalGroup("New Translation/Create Buttons"), Button]
        private static void NewStringTranslation()
        {
            // if (string.IsNullOrEmpty(NewTranslationName)) return;
            // string fullPath = PathToStoreTranslations + "/" + NewTranslationName + ".asset";
            CreateNewTranslationByName(FullTranslationName, TranslationType.String);
            LastCreatedTranslation = _translationsList.Find(x => x.name == FullTranslationName);
        }
        
        [BoxGroup("New Translation"), HorizontalGroup("New Translation/Create Buttons"), Button]
        private static void NewSpriteTranslation()
        {
            CreateNewTranslationByName(FullTranslationName, TranslationType.Sprite);
            LastCreatedTranslation = _translationsList.Find(x => x.name == FullTranslationName);
        }

        [BoxGroup("New Translation"), HorizontalGroup("New Translation/Create Buttons"), Button]
        private static void NewAudioTranslation()
        {
            CreateNewTranslationByName(FullTranslationName, TranslationType.AudioClip);
            LastCreatedTranslation = _translationsList.Find(x => x.name == FullTranslationName);
        }
        
        private static void CreateNewTranslationByName(string fileName, TranslationType type)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            string fullPath = PathToStoreTranslations + "/" + fileName + ".asset";

            if (System.IO.File.Exists(fullPath))
            {
                EditorUtility.DisplayDialog("Translation exists!", "A translation already exists with this name!", "Cancel");
                return;
            }

            CreateTranslationScriptableObject(fileName, type);
        }

        private static void CreateTranslationScriptableObject(string fileName, TranslationType type)
        {
            string fullPath = PathToStoreTranslations + "/" + fileName + ".asset";

            if (System.IO.File.Exists(fullPath))
            {
                //Do nothing when exists!
                return;
            }
            
            string relativePath = PathToStoreTranslations.Substring(PathToStoreTranslations.IndexOf("Assets/"));

            switch (type)
            {
                case TranslationType.String:
                    TranslationAssetString newStringTranslation = CreateInstance<TranslationAssetString>();
                    newStringTranslation.translations = new TranslationAssetString.TranslationString[Settings.defaultLanguageSet.Count];
                    for (int i = 0; i < Settings.defaultLanguageSet.Count; i++)
                    {
                        newStringTranslation.translations[i] = new TranslationAssetString.TranslationString
                        {
                            language = Settings.defaultLanguageSet[i]
                        };
                    }
                    AssetDatabase.CreateAsset(newStringTranslation, relativePath + "/" + fileName + ".asset");
                    break;
                case TranslationType.Sprite:
                    TranslationAssetSprite newSpriteTranslation = CreateInstance<TranslationAssetSprite>();
                    newSpriteTranslation.translations = new TranslationAssetSprite.TranslationSprite[Settings.defaultLanguageSet.Count];
                    for (int i = 0; i < Settings.defaultLanguageSet.Count; i++)
                    {
                        newSpriteTranslation.translations[i] = new TranslationAssetSprite.TranslationSprite
                        {
                            language = Settings.defaultLanguageSet[i]
                        };
                    }
                    AssetDatabase.CreateAsset(newSpriteTranslation, relativePath + "/" + fileName + ".asset");
                    break;
                case TranslationType.AudioClip:
                    TranslationAssetAudioClip newAudioClipTranslation = CreateInstance<TranslationAssetAudioClip>();
                    newAudioClipTranslation.translations = new TranslationAssetAudioClip.TranslationAudioClip[Settings.defaultLanguageSet.Count];
                    for (int i = 0; i < Settings.defaultLanguageSet.Count; i++)
                    {
                        newAudioClipTranslation.translations[i] = new TranslationAssetAudioClip.TranslationAudioClip
                        {
                            language = Settings.defaultLanguageSet[i]
                        };
                    }
                    AssetDatabase.CreateAsset(newAudioClipTranslation, relativePath + "/" + fileName + ".asset");
                    break;
            }
            
            AssetDatabase.Refresh();
            AssetDatabase.ImportAsset(relativePath + "/" + fileName);
            LoadTranslations();
        }
        
        public static List<T> FindAllScriptableObjectsOfType<T>(string filter, string folder = "Assets")
            where T : ScriptableObject
        {
            if (string.IsNullOrEmpty(folder)) return new List<T>();
            
            
            string relativePath = folder.Substring(folder.IndexOf("Assets/"));

            if (!AssetDatabase.IsValidFolder(relativePath))
            {
                _settings.pathToStoreTranslations = EditorUtility.OpenFolderPanel(
                    "No path set",
                    "Select the folder you want to store your localizations in. This needs to be withing your Assets folder",
                    "Localization");
                relativePath = _settings.pathToStoreTranslations.Substring(folder.IndexOf("Assets/"));
            }
            
            return AssetDatabase.FindAssets(filter, new[] { relativePath })
                .Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToList();
        }
        
        void OnInspectorUpdate()
        {
            if (Settings == null || _translationsList.Count == 0)
            {
                LoadTranslations();
            }
            Repaint();
        }
    }
}
