$FileName1 = 'output_journeys_too_short.csv'
$FileName2 = 'output_journeys_dataok.csv'

Import-CSV -Delimiter ',' 'datasets/journey/2021-05.csv' | ForEach-Object {
 
    #Current row object
    $CSVRecord = $_

    #Read column values in the current row 
    $Duration = $CSVRecord.'Duration (sec.)'
    $Distance = $CSVRecord.'Covered distance (m)'

    if ($Distance -lt 10 -and $Duration -lt 10) {
        $CSVRecord | Export-Csv -Path $FileName1 -NoTypeInformation -Append
    }

    if ($Distance -gt 10 -and $Duration -gt 10) {
        $CSVRecord | Export-Csv -Path $FileName2 -NoTypeInformation -Append
    }
}