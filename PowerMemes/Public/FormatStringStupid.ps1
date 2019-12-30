function Format-StringStupid {
    [CmdletBinding()]
    param (
        # Text to stupify
        [Parameter(Mandatory, ValueFromPipeline)]
        [ValidateNotNullOrEmpty()]
        [string] $String
    )

    begin {
        [System.Func[char, System.Object]] $SelectFunction = {
            param (
                [Parameter(Mandatory)]
                [char] $c
            )
            $Rando = Get-Random -Minimum 0 -Maximum 9

            Write-Verbose "Rando: $Rando"
            Write-Verbose "Input Char: $c"

            if ($Rando -lt 6) {
                $Output = [System.Char]::ToLowerInvariant($c)    
            }
            else {
                $Output = [System.Char]::ToUpperInvariant($c)
            }

            Write-Verbose "Output: $Output"
            return $Output
        }
      
    }
        
    process {
        $Chars = $String.ToCharArray()      
        
        $StupidQuery = [System.Linq.Enumerable]::Select($Chars, $SelectFunction)
        
        [System.Linq.Enumerable]::ToArray($StupidQuery) -join ""        
    }
   
}