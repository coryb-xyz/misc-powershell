@{
    # If authoring a script module, the RootModule is the name of your .psm1 file
    RootModule = 'MemePowershell.psm1'

    Author = 'Cory <contact@coryb.xyz>'

    CompanyName = 'coryb.xyz'

    ModuleVersion = '1.0'

    # Use the New-Guid command to generate a GUID, and copy/paste into the next line
    GUID = 'e135020a-f4cc-441e-a309-27a2670a6d02'

    Copyright = '2020 Copyright coryb.xyz'

    Description = 'For memes'

    # Minimum PowerShell version supported by this module (optional, recommended)
    # PowerShellVersion = ''

    # Which PowerShell Editions does this module work with? (Core, Desktop)
    CompatiblePSEditions = @('Desktop')

    # Which PowerShell functions are exported from your module? (eg. Get-CoolObject)
    FunctionsToExport = @('Format-StringStupid', 'New-SpongeBobMeme')

    # Which PowerShell aliases are exported from your module? (eg. gco)
    AliasesToExport = @('')

    # Which PowerShell variables are exported from your module? (eg. Fruits, Vegetables)
    VariablesToExport = @('')

    # PowerShell Gallery: Define your module's metadata
    PrivateData = @{
        PSData = @{
            # What keywords represent your PowerShell module? (eg. cloud, tools, framework, vendor)
            Tags = @('memes', 'spongebob')

            # What software license is your code being released under? (see https://opensource.org/licenses)
            LicenseUri = ''

            # What is the URL to your project's website?
            ProjectUri = ''

            # What is the URI to a custom icon file for your project? (optional)
            IconUri = ''

            # What new features, bug fixes, or deprecated features, are part of this release?
            ReleaseNotes = @'
'@
        }
    }

    # If your module supports updateable help, what is the URI to the help archive? (optional)
    # HelpInfoURI = ''
}