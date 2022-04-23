import os
import json
import csv

tempDirectory = os.environ['TEMP']
pathToTraining = os.path.join(tempDirectory, 'training-actions.csv')
header = [ 'Action', 'Resource', 'Amount', 'Transferring', 'Receiving', 'EU' ]

data = []

f = open(os.path.join(tempDirectory, 'output-schedules.json'))
schedules = json.load(f)

for s in schedules:
    previousEU = 0
    for a in s['Actions']:
        row = []
        if a['Type'] == 'transfer':
            row.append('Transfer')
            row.append(a['Resource'])
            row.append(a['Amount'])
            row.append(a['Transferring Country'])
            row.append(a['Receiving Country'])
        else:
            row.append('Transform')
            for i in a['Outputs']:
                if 'Population' not in i and 'Waste' not in i:
                    row.append(i)
                    row.append(a['Outputs'][i])
            row.append(a['Country'])
            row.append(a['Country'])
        row.append(round(a['Expected Utility'] - previousEU, 2))
        data.append(row)
        previousEU = a['Expected Utility']

with open(pathToTraining, 'a+', encoding='UTF8', newline='') as f:
    reader = csv.reader(f)
    writer = csv.writer(f)

    if (len(list(reader)) == 0):
        writer.writerow(header)
        
    writer.writerows(data)