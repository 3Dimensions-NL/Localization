# 3Dimensions Localization
A Unity plugin to streamline localization in your projects with a focus on simplicity, flexibility, and compatibility. Create and manage translations for UI text, sprites, and audio effortlessly.

## Prerequisites
- Unity 2022.3.x LTS or higher.
- Installed TextMesh Pro package (via Package Manager).

## Installation
1. Open Unity and navigate to `Window -> Package Manager`.
2. Click the **+ Add** button in the top-left corner and select **"Add package from Git URL"**.
3. Paste the Git repository URL and click **Add**.
4. Import Samples to get demo assets and a manager script (this can be done from the Package Manager window under "Samples").

## Usage
1. Open the **Localization Editor Window** `3Dimensions -> Localization -> Localization Editor`.
2. Select a translation folder where you want your assets to be saved.
3. Configure settings in the **Localization Settings**:
    - Add or remove supported languages.
    - Set a default language.
4. Create translations in the **Translation Creation** section:
    - Enter a Translation name.
    - Click one of the New Translation Type buttons (String, Sprite, or Audio).
5. Import translations from a CSV file (ensure language headers match exactly).
6. Add the `LocalizationManager` prefab to your start scene. This will persist through scenes and manage Runtime translations.
7. Attach a `TranslationComponent` to any localizable object (e.g., `TextMesh Pro`, Sprite, or AudioSource). Assign a translation asset and test it with **Apply Translation**.
8. Change the language during runtime using the `LanguageSelectButton` prefab or:
   ```csharp
   LocalizationManager.SetLanguage("German");
   ```

## Notes
1. A demo scene **LocalizationSampleScene** is included in the samples folder.
2. Sprite and audio translations require you to link existing files in your project.

## Tips & Common Issues
- Ensure all translation assets have unique names.
- Import/export translations in CSV format to manage them efficiently.