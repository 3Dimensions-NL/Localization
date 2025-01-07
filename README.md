# 3Dimensions Localization
A collection of components and scripts to help localize your Unity project.

## Usage
1. Install the package with the Unity Package Manager. (Use the 'Install package from GIT URL' function and paste the repo URL).
2. Import the Samples (these contain a manager script and example translation assets).
3. Open the Localization Editor Window via the menu (3Dimensions -> Localization -> Localization Editor) and select a folder where you want your translations assets to be saved.
4. Change the Localization Settings if to your liking, add new languages or change the default language.
5. Under Translation Creation, enter a Translation name and hit one of the New Translation Type buttons (for now it's just string, sprite and audio). 
6. You can also import a translation csv file. (Remember to use the correct language names in the header! You can also create some string translations and export a csv to see the structure)
7. When you have created some translations you can add the LocalizationManager prefab to your start scene. This wil manage all TranslationComponents and will not destroy on load.
8. Add a TranslationComponent to an translatable object (e.g. TextMesh Pro Text object, sprite or audio source) and add one of your translation assets and click the **Apply Translation** or **Switch Translation** buttons to test it out.
9. If you want to change the language in runtime you can use the LanguageSelectButton from the samples or create a dropdown that sets the language via the LocalizationManager.

## Notes
1. There is a demo scene **'LocalizationSampleScene'** in the samples folder to showcase a simple setup. 
2. For sprite and audio translation assets you have to save the actual files somewhere in your project and link them to the Translation Asset. See the Sprite_Flag_Translation.asset as an example.

