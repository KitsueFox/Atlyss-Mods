using System.Resources;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;
using TextureReplacer;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle(TextureReplacer.BuildInfo.Name)]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany(TextureReplacer.BuildInfo.Company)]
[assembly: AssemblyProduct(TextureReplacer.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created By " + TextureReplacer.BuildInfo.Author)]
[assembly: AssemblyTrademark(TextureReplacer.BuildInfo.Company)]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("FD3630D8-19E6-4819-9B3B-FB27B336FA50")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion(TextureReplacer.BuildInfo.Version)]
[assembly: AssemblyFileVersion(TextureReplacer.BuildInfo.Version)]
[assembly: NeutralResourcesLanguage("en")]

// Melonloader
[assembly: MelonInfo(typeof(Texture_Replacer), "Texture Replacement", "0.0.1", "BlackyFox")]
[assembly: MelonGame("KisSoft", "ATLYSS")]