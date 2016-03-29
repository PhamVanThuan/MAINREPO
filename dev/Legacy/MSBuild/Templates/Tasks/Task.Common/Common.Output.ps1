function Write-Pass($textToWrite)
{
    Write-Host "[PASS]...$textToWrite" -ForegroundColor Green   
}

function Write-Fail($textToWrite)
{
    Write-Host "$[FAILURE]...$textToWrite" -ForegroundColor Red   
}

function Write-Begin-Check($checkComment)
{
    Write-Host "$checkComment..."
}

function Write-Check($checkComment)
{
    Write-Host "$checkComment..."
}