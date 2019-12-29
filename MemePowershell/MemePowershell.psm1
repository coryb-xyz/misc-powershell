$Public = @( Get-ChildItem -Path $PSScriptRoot\Public\*.ps1 -ErrorAction Stop )

foreach ($import in $Public) {
    Try
    {
        . $import.fullname
    }
    Catch
    {
        Write-Error -Message "Failed to import function $($import.fullname): $_"
    }
}

Export-ModuleMember -Function $Public.Basename