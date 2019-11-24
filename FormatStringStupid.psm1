function Format-StringStupid {
    [CmdletBinding()]
    param (
        # Text to stupify
        [Parameter(Mandatory, ValueFromPipeline)]
        [string]
        $String
    )
        
    process {
        $Chars = $String.ToCharArray()

        [System.Func[char, System.Object]] $SelectFunction = {
            param (
                [Parameter(Mandatory)]
                [char]
                $c
            )
            $Rando = Get-Random -Minimum 0 -Maximum 9
            if ($Rando -lt 5) {
                [System.Char]::ToLowerInvariant($c)
            }
            else {
                [System.Char]::ToUpperInvariant($c)
            }
        }
        
        $StupidQuery = [System.Linq.Enumerable]::Select($Chars, $SelectFunction)
        
        [System.Linq.Enumerable]::ToArray($StupidQuery) -join "" 
    }
   
}