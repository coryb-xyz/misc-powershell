function Format-StringStupid {
    [CmdletBinding()]
    param (
        # Text to stupify
        [Parameter(Mandatory, ValueFromPipeline)]
        [ValidateNotNullOrEmpty()]
        [string] $String,
        
        [Parameter()]
        [bool] $Copy
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

            if ($Rando -lt 5) {
                $Output = [System.Char]::ToLowerInvariant($c)    
            }
            else {
                $Output = [System.Char]::ToUpperInvariant($c)
            }

            Write-Verbose "Output: $Output"
            return $Output
        }

        $Signature = @'
        [DllImport("user32.dll")]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);
    
        [DllImport("user32.dll")]
        public static extern bool CloseClipboard();
    
        [DllImport("user32.dll")]
        public static extern bool SetClipboardData(uint uFormat, IntPtr data);
'@

      $WinUtils = Add-Type -MemberDefinition $Signature -Name Win32Utils -Namespace PInvoke -Using PInvoke, System.Runtime.InteropServices
    }
        
    process {
        $Chars = $String.ToCharArray()      
        
        $StupidQuery = [System.Linq.Enumerable]::Select($Chars, $SelectFunction)
        
        $Result = [System.Linq.Enumerable]::ToArray($StupidQuery) -join ""

        if ($Copy) {
            Write-Verbose "Copying to system clipboard"

            $WinUtils::CloseClipboard([System.IntPtr]::Zero)

            $PTR = [System.Runtime.InteropServices.Marshal]::StringToHGlobalUni($Result)

            $WinUtils::SetClipboardData(13, $PTR)

            $WinUtils::CloseClipboard()

            $WinUtils::FreeHGlobal($PTR)
        }

        return $Result
        
    }
   
}