C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild reports.msbuild /t:CodeCoverage

copy "Reports\MSpec\*" "C:\inetpub\wwwroot\DomainService\Specs" 

copy "Reports\Coverage\*" "C:\inetpub\wwwroot\DomainService\Coverage" 
 