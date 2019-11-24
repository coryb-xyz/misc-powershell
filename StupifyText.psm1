function Stupify-String {
    [CmdletBinding()]
    param (
        # Text to stupify
        [Parameter(Mandatory)]
        [string]
        $String
    )
    
    begin {
        
    }
    
    process {
        $Chars = $String.ToCharArray()
        [System.Func[char, System.Object]] $SelectDelegate = {
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
        $StupidQuery = [System.Linq.Enumerable]::Select($Chars, $SelectDelegate)
        $StupidArray = [System.Linq.Enumerable]::ToArray($StupidQuery)

        $StupidArray -join ""
        
    }
    
    end {
        
    }
}