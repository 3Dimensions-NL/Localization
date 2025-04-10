using System;
using System.Collections.Generic;
using System.Linq;
using _3Dimensions.Localization.Runtime.Scripts;
using _3Dimensions.Localization.Runtime.Scripts.Translations;
using UnityEditor;
using UnityEngine;
namespace _3Dimensions.Localization.Editor.Scripts
{
    public class LocalizationEditor : EditorWindow 
    {
        public enum TranslationType
        {
            String,
            Sprite,
            AudioClip
        }
        
        private static readonly string _settingsPath = "LocalizationSettings";
        
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


        private static LocalizationSettings _settings;
        
        public static string NewTranslationName;

        private static string PathToStoreTranslations => Settings.pathToStoreTranslations;
        
        private static List<TranslationAsset> _translationsList = new List<TranslationAsset>();

        private GUIStyle colorButtonStyle;
        private Color originalBackgroundColor;

        private static Color darkBlue = new Color(0.4f, 0.6f, 1f);
        private static Color darkGreen = new Color(0.3f, 1f, 0.4f);
        private static Color darkYellow = new Color(1f, .9f, 0.5f);

        private bool _showLocalizationSettings = true; // To control the collapsing of settings
        private bool _showNewTranslationButtons = true; // To control the collapsing of new translation buttons
        private bool _showImportExport = true; // To control the collapsing of import/export buttons
        
        private Vector2 _mainScrollPosition; // To manage the scroll position for the entire window
        private Vector2 _translationScrollPos; // To manage the scroll position of the translations list
        
        
        [MenuItem("3Dimensions/Localization/Localization Editor")]
        private static void OpenWindow()
        {
            LocalizationEditor window = (LocalizationEditor)GetWindow(typeof(LocalizationEditor));
            window.titleContent = new GUIContent("Localization Editor");
            LoadSettings();

            if (!System.IO.Directory.Exists(_settings.pathToStoreTranslations))
            {
                string newPath = EditorUtility.OpenFolderPanel("Path to save localizations",  Application.dataPath + "/Localizations", null);
                if (!string.IsNullOrEmpty(newPath)) _settings.pathToStoreTranslations = newPath;
            }
        }

        private void OnGUI()
        {
            CreateStyles();

            // Begin a vertical scroll view for the entire window
            _mainScrollPosition = EditorGUILayout.BeginScrollView(_mainScrollPosition, GUILayout.ExpandHeight(true));
            
            originalBackgroundColor = GUI.backgroundColor;
            
            LoadLocalizationSettings();

            EditorGUILayout.BeginVertical("box");
            _showLocalizationSettings = EditorGUILayout.BeginFoldoutHeaderGroup(_showLocalizationSettings, "Localization Settings", EditorStyles.foldoutHeader);
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (_showLocalizationSettings)
            {
                DrawLocalizationSettings();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            _showNewTranslationButtons = EditorGUILayout.BeginFoldoutHeaderGroup(_showNewTranslationButtons, "Translation Creation", EditorStyles.foldoutHeader);
            EditorGUILayout.EndFoldoutHeaderGroup();

            if (_showNewTranslationButtons)
            {
                EditorGUILayout.BeginVertical("box");
                DrawNewTranslationButtons();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();

            // EditorGUILayout.BeginVertical("box");
            // _showTranslationEditor = EditorGUILayout.BeginFoldoutHeaderGroup(_showTranslationEditor, "Translation Editor", EditorStyles.foldoutHeader);
            // EditorGUILayout.EndFoldoutHeaderGroup();
            //
            // if (_showTranslationEditor)
            // {
            //     EditorGUILayout.BeginVertical("box");
            //     DrawTranslationEditor(); // Ensure this has no nesting!
            //     EditorGUILayout.EndVertical();
            // }
            //
            // EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");
            _showImportExport = EditorGUILayout.BeginFoldoutHeaderGroup(_showImportExport, "Import/Export", EditorStyles.foldoutHeader);
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (_showImportExport)
            {
                // Draw import/export buttons
                EditorGUILayout.BeginVertical("box");
                DrawImportExport();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("box");
            
            // Draw translation list
            DrawTranslationList();

            EditorGUILayout.EndVertical();

            // End the vertical scroll view
            EditorGUILayout.EndScrollView();
        }

        private static void LoadSettings()
        {
            LoadTranslations();
        }

        private static void LoadLocalizationSettings()
        {
            if (_settings == null)
            {
                Debug.Log("Loading LocalizationSettings");
                _settings = Resources.Load<LocalizationSettings>(_settingsPath);
            }
        }

        private static void DrawLocalizationSettings()
        {
            if (_settings == null)
            {
                GUILayout.Label("No Localization Settings found", EditorStyles.boldLabel);
                return;
            }
            
            EditorGUI.BeginChangeCheck();
            
            GUILayout.Label("Current Localization Settings", EditorStyles.boldLabel);
            _settings = (LocalizationSettings)EditorGUILayout.ObjectField("Settings Asset", _settings, typeof(LocalizationSettings), false);

            GUILayout.Label("Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            _settings.pathToStoreLanguages = EditorGUILayout.TextField("Language path", _settings.pathToStoreLanguages);
            if (GUILayout.Button("Set", GUILayout.Width(50)))
            {
                string newPath = EditorUtility.OpenFolderPanel("Set Language Path", _settings.pathToStoreLanguages, "");
                if (!string.IsNullOrEmpty(newPath))
                {
                    _settings.pathToStoreLanguages = newPath;
                }
            }
            EditorGUILayout.EndHorizontal();

            // **Path to Store Translations**
            EditorGUILayout.BeginHorizontal();
            _settings.pathToStoreTranslations = EditorGUILayout.TextField("Translations path", _settings.pathToStoreTranslations);
            if (GUILayout.Button("Set", GUILayout.Width(50)))
            {
                string newPath = EditorUtility.OpenFolderPanel("Set Translations Path", _settings.pathToStoreTranslations, "");
                if (!string.IsNullOrEmpty(newPath))
                {
                    _settings.pathToStoreTranslations = newPath;
                }
            }
            EditorGUILayout.EndHorizontal();

            // Aanpassing van Default Language Set
            GUILayout.Label("Languages", EditorStyles.boldLabel);
            
            // Standaardtaal instellen
            _settings.defaultLanguage = (LanguageObject)EditorGUILayout.ObjectField(
                "Default Language",
                _settings.defaultLanguage,
                typeof(LanguageObject),
                false
            );
            
            if (_settings.languageSet != null && _settings.languageSet.Count > 0)
            {
                for (int i = 0; i < _settings.languageSet.Count; i++)
                {
                    GUILayout.BeginHorizontal();
                    _settings.languageSet[i] = (LanguageObject)EditorGUILayout.ObjectField(
                        $"Language {i + 1}",
                        _settings.languageSet[i],
                        typeof(LanguageObject),
                        false
                    );

                    if (GUILayout.Button("Remove", GUILayout.Width(70)))
                    {
                        _settings.languageSet.RemoveAt(i);
                    }
                    GUILayout.EndHorizontal();
                }
            }
            else
            {
                GUILayout.Label("No default languages set.");
            }

            // Knop om een nieuwe taal toe te voegen
            if (GUILayout.Button("Add language"))
            {
                _settings.languageSet.Add(null); // Voeg een lege taal toe
            }

            // Detecteer wijzigingen en markeer ScriptableObject als veranderd
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(_settings); // Markeer het ScriptableObject als gewijzigd
            }
        }

        private void DrawNewTranslationButtons()
        {
            // Text field for New Translation Name
            NewTranslationName = EditorGUILayout.TextField("Translation Name", NewTranslationName);

            // Dynamic layout based on the width of the editor window
            float windowWidth = position.width;

            if (windowWidth < 460) // Threshold for switching from horizontal to vertical layout
            {
                // Stack buttons vertically
                GUILayout.BeginVertical();
            }
            else
            {
                // Display buttons side by side
                GUILayout.BeginHorizontal();
            }

            GUI.color = darkBlue;

            if (GUILayout.Button("New String Translation"))
            {
                NewStringTranslation();
            }

            GUI.color = originalBackgroundColor;
            GUI.color = darkGreen;

            if (GUILayout.Button("New Sprite Translation"))
            {
                NewSpriteTranslation();
            }

            GUI.color = originalBackgroundColor;
            GUI.color = darkYellow;

            if (GUILayout.Button("New Audio Translation"))
            {
                NewAudioTranslation();
            }

            if (windowWidth < 460)
            {
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.EndHorizontal();
            }
            
            GUI.color = originalBackgroundColor;
        }

        private void DrawImportExport()
        {
            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("Import CSV"))
            {
                ImportCSV();
            }
            if (GUILayout.Button("Export CSV"))
            {
                ExportCSV();
            }
            EditorGUILayout.EndHorizontal();
        }
        
        private string _searchQuery = ""; // To hold the search input from the user

        private void DrawTranslationList()
        {
            GUILayout.Label("Translations", EditorStyles.boldLabel);

            // Add a search bar
            EditorGUILayout.BeginHorizontal();
            _searchQuery = EditorGUILayout.TextField("Search", _searchQuery);
            if (GUILayout.Button("Clear", GUILayout.Width(50))) // Clear button for reset
            {
                _searchQuery = ""; // Clear search query
            }

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(4); // Add some spacing between the search bar and the list

            // Count assets of each type
            int totalCount = _translationsList.Count;
            int stringCount = _translationsList.Count(t => t is TranslationAssetString);
            int spriteCount = _translationsList.Count(t => t is TranslationAssetSprite);
            int audioClipCount = _translationsList.Count(t => t is TranslationAssetAudioClip);

            // Display counts
            GUILayout.Label(
                $"Assets Count {totalCount} (Strings: {stringCount} | Sprites: {spriteCount} | Audio Clips: {audioClipCount})");

            // Begin a scrollview that automatically adapts to window height
            _translationScrollPos = EditorGUILayout.BeginScrollView(_translationScrollPos, GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));

            if (_translationsList != null && _translationsList.Count > 0)
            {
                // Filter the translations based on the search query
                List<TranslationAsset> filteredTranslations = _translationsList
                    .Where(t => t != null && (string.IsNullOrEmpty(_searchQuery) ||
                                              t.name.IndexOf(_searchQuery, StringComparison.OrdinalIgnoreCase) >= 0))
                    .ToList();

                if (filteredTranslations.Count > 0)
                {
                    // Track for deletion
                    TranslationAsset toDelete = null;

                    foreach (var translation in filteredTranslations)
                    {
                        if (translation == null) continue; // Skip any null references

                        EditorGUILayout.BeginHorizontal();

                        // Display the translation asset
                        EditorGUILayout.ObjectField(translation, typeof(TranslationAsset), false);

                        // Add the Edit button
                        if (GUILayout.Button("Edit", GUILayout.Width(50)))
                        {
                            // Instead of loading the TranslationEditor, focus the Inspector
                            Selection.activeObject = translation; // Highlight the translation in the Inspector
                            EditorGUIUtility
                                .PingObject(translation); // Make sure the object is pinged in the Project view
                        }

                        // Add the Delete button with a cross icon and a tooltip
                        GUIContent deleteContent = new GUIContent("x", "Delete this translation");
                        if (GUILayout.Button(deleteContent, GUILayout.Width(25)))
                        {
                            toDelete = translation; // Mark the item for deletion
                        }

                        EditorGUILayout.EndHorizontal();
                    }

                    // Remove the translation marked for deletion
                    if (toDelete != null)
                    {
                        // Confirm the deletion
                        bool confirmDelete = EditorUtility.DisplayDialog(
                            "Delete Translation",
                            $"Are you sure you want to delete the translation '{toDelete.name}'?",
                            "Yes",
                            "No"
                        );

                        if (confirmDelete)
                        {
                            string removedName = toDelete.name;
                            // Remove it from the translations list
                            _translationsList.Remove(toDelete);

                            // Delete the actual asset
                            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(toDelete));
                            Debug.Log($"Deleted translation: {removedName}");

                            AssetDatabase.Refresh();
                        }
                    }
                }
                else
                {
                    GUILayout.Label("No translations match your search query.");
                }
            }
            else
            {
                GUILayout.Label("No translations found.");
            }

            EditorGUILayout.EndScrollView();
        }

        private void ImportCSV()
        {
            string filePath = EditorUtility.OpenFilePanel("Import CSV", Application.dataPath, "csv");
            if (string.IsNullOrEmpty(filePath)) return;

            try
            {
                var data = CsvFileReaderWriter.ReadCsv(filePath);

                if (data.Count == 0)
                {
                    Debug.LogError("The CSV file is empty. Nothing to import.");
                    return;
                }

                // Example: Assuming headers and rows as processed in your previous ImportCSV
                List<LanguageObject> importedLanguages = new List<LanguageObject>();
                int headerCount = data[0].Count;

                for (int i = 1; i < data.Count; i++) // Skip the first row (header)
                {
                    List<string> row = data[i];
                    // Perform your logic to handle each row of translations
                    string translationName = row[0]; // Translation identifier
                    CreateTranslationScriptableObject(translationName, TranslationType.String);

                    TranslationAsset translation = _translationsList.Find(x => x.name == translationName);
                    if (translation is TranslationAssetString translationAssetString)
                    {
                        translationAssetString.translations =
                            new TranslationAssetString.TranslationString[headerCount - 1];

                        for (int columnIndex = 1; columnIndex < headerCount; columnIndex++)
                        {
                            if (columnIndex - 1 >= Settings.languageSet.Count) continue;

                            TranslationAssetString.TranslationString translationString =
                                new TranslationAssetString.TranslationString
                                {
                                    language = Settings.languageSet[columnIndex - 1],
                                    text = row[columnIndex]
                                };

                            translationAssetString.translations[columnIndex - 1] = translationString;
                        }

                        EditorUtility.SetDirty(translationAssetString);
                    }
                }

                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to import CSV: {ex.Message}");
            }
        }

        private void ExportCSV()
        {
            string filePath = EditorUtility.SaveFilePanel("Export CSV", Application.dataPath, "translations", "csv");
            if (string.IsNullOrEmpty(filePath)) return;

            try
            {
                var csvData = new List<List<string>>();

                // Write headers
                List<string> headers = new List<string> { "Translation Name" };
                foreach (var lang in Settings.languageSet)
                {
                    headers.Add(lang.name);
                }

                csvData.Add(headers);

                // Write translation rows
                foreach (var translationAsset in _translationsList)
                {
                    if (translationAsset is TranslationAssetString translationAssetString)
                    {
                        List<string> row = new List<string> { translationAssetString.name };
                        foreach (var lang in Settings.languageSet)
                        {
                            var translation =
                                translationAssetString.translations.FirstOrDefault(t => t.language == lang);
                            string value = translation?.text ?? ""; // Default to empty string if no translation found
                            row.Add(value);
                        }

                        csvData.Add(row);
                    }
                }

                CsvFileReaderWriter.WriteCsv(csvData, filePath);
                Debug.Log("CSV export completed!");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to export CSV: {ex.Message}");
            }
        }

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

        private static void NewStringTranslation()
        {
            CreateAndSelectNewTranslation("String_", TranslationType.String);
        }

        private static void NewSpriteTranslation()
        {
            CreateAndSelectNewTranslation("Sprite_", TranslationType.Sprite);
        }

        private static void NewAudioTranslation()
        {
            CreateAndSelectNewTranslation("Audio_", TranslationType.AudioClip);
        }
        
        private static void CreateAndSelectNewTranslation(string prefix, TranslationType type)
        {
            // Create the translation asset
            CreateNewTranslationByName(prefix + NewTranslationName, type);

            // Find and select the newly created translation asset
            var newAsset = _translationsList.Find(x => x.name == prefix + NewTranslationName);
            if (newAsset != null)
            {
                Selection.activeObject = newAsset; // Set the asset as active in the inspector
                EditorGUIUtility.PingObject(newAsset); // Highlight it in the Project window
            }
        }
        
        private static void CreateNewTranslationByName(string fileName, TranslationType type)
        {
            if (string.IsNullOrEmpty(NewTranslationName))
            {
                NewTranslationName = "NewTranslation";
                fileName = fileName + NewTranslationName;
            }

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
                    newStringTranslation.translations = new TranslationAssetString.TranslationString[Settings.languageSet.Count];
                    for (int i = 0; i < Settings.languageSet.Count; i++)
                    {
                        newStringTranslation.translations[i] = new TranslationAssetString.TranslationString
                        {
                            language = Settings.languageSet[i]
                        };
                    }
                    AssetDatabase.CreateAsset(newStringTranslation, relativePath + "/" + fileName + ".asset");
                    break;
                case TranslationType.Sprite:
                    TranslationAssetSprite newSpriteTranslation = CreateInstance<TranslationAssetSprite>();
                    newSpriteTranslation.translations = new TranslationAssetSprite.TranslationSprite[Settings.languageSet.Count];
                    for (int i = 0; i < Settings.languageSet.Count; i++)
                    {
                        newSpriteTranslation.translations[i] = new TranslationAssetSprite.TranslationSprite
                        {
                            language = Settings.languageSet[i]
                        };
                    }
                    AssetDatabase.CreateAsset(newSpriteTranslation, relativePath + "/" + fileName + ".asset");
                    break;
                case TranslationType.AudioClip:
                    TranslationAssetAudioClip newAudioClipTranslation = CreateInstance<TranslationAssetAudioClip>();
                    newAudioClipTranslation.translations = new TranslationAssetAudioClip.TranslationAudioClip[Settings.languageSet.Count];
                    for (int i = 0; i < Settings.languageSet.Count; i++)
                    {
                        newAudioClipTranslation.translations[i] = new TranslationAssetAudioClip.TranslationAudioClip
                        {
                            language = Settings.languageSet[i]
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

        private void CreateStyles()
        {
            if (colorButtonStyle == null)
            {
                // Maak een nieuwe stijl aan
                colorButtonStyle = new GUIStyle(GUI.skin.button);

                // Pas de achtergrondkleur aan, donkerblauw
                colorButtonStyle.normal.textColor = Color.white;
            }
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
