$gameGuideHTML = Invoke-RestMethod -Uri "https://www.nintendo.com/games/game-guide/" 
$keyExists = $gameGuideHTML -match "\s*searchApiKey: `"(\w+)`"" 
$apiKey = if ($keyExists) {
    $Matches[1]
}
else {
    throw "Uh oh, couldn't get the api key from Nintendo :("
}

function UrlEncodeParameters {
    param (
        [hashtable]
        $Params
    )
    return  "query=&{0}" -f ($Params.Keys.ForEach( {
                "{0}={1}" -f [System.Web.HttpUtility]::UrlEncode($_), [System.Web.HttpUtility]::UrlEncode($Params[$_])
            }) -join "&")
}

$onLastPage = $false
$page = 0
$hits = while (-not $onLastPage) {
    $hitsPerPage = 500
    $requests = @(
        @{
            indexName = "ncom_game_en_us"
            params    = UrlEncodeParameters @{
                hitsPerPage       = $hitsPerPage
                maxValuesPerFacet = 100
                page              = $page++
                analytics         = "false"
                facets            = , @(
                    "generalFilters"
                    "platform"
                    "availability"
                    "genres"
                    "howToShop"
                    "virtualConsole"
                    "franchises"
                    "priceRange"
                    "esrbRating"
                    "playerFilters"
                ) | ConvertTo-Json -Compress
                tagFilters        = ""
                facetFilters      = , @(@("platform:Nintendo Switch"), @("generalFilters:Deals")) | ConvertTo-Json -Compress
            }
        }  
    )

    $requestBody = @{
        requests = $requests
    }

    $querySplat = @{
        Body        = $requestBody | ConvertTo-Json -Compress
        Method      = "POST" 
        ContentType = "application/x-www-form-urlencoded"
        Uri         = "https://u3b6gr4ua3-dsn.algolia.net/1/indexes/*/queries?x-algolia-agent=Algolia%20for%20JavaScript%20(3.33.0)%3B%20Browser%20(lite)%3B%20JS%20Helper%202.20.1&x-algolia-application-id=U3B6GR4UA3&x-algolia-api-key={0}" -f $apiKey 

    }
    $results = (Invoke-RestMethod @querySplat).results.hits
    $onLastPage = $results.count -ne $hitsPerPage
    $results
}
return $hits | Select-Object title, description, salePrice, msrp, @{
    n = "discount"
    e = { [System.Math]::Round((1 - [short]$_.salePrice / [short]$_.msrp) * 100, 2) } 
}, @{
    n = "url"
    e = { "https://www.nintendo.com{0}" -f $_.url }
},esrbRating,numOfPlayers, @{
    n = "releaseDate"
    e = {Get-Date $_.releaseDateDisplay}
}
    