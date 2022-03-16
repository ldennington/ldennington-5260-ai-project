import os
import json

tempDirectory = os.environ['TEMP']

f = open(os.path.join(tempDirectory, 'output-schedules.json'))
schedules = json.load(f)

# The formatted output in output-schedules-formatted.txt is adapted from
#   Alex Nelson's explanation of his solution at
#   https://piazza.com/class/kyz01i5gip25bn?cid=95_f1 */
formatted = []
pieces = []
for s in schedules:
    for a in s['Actions']:
        if a['Type'] == 'transform':
            pieces.append(f'TRANSFORM {a["Country"]} INPUTS (')
            for i in a['Inputs']:
                pieces.append(f'({i} {a["Inputs"][i]})')
            pieces.append(') OUTPUTS (')
            for i in a['Outputs']:
                pieces.append(f'({i} {a["Outputs"][i]})')
            pieces.append(f') EU: {a["Expected Utility"]}\n')
        if a['Type'] == 'transfer':
            pieces.append(f'TRANSFER FROM {a["Transferring Country"]} TO {a["Receiving Country"]} ({a["Resource"]} ' +
                 f'{a["Amount"]}) EU: {a["Expected Utility"]}\n')
    formatted.append(''.join(pieces))
    pieces.clear()

with open(os.path.join(tempDirectory, 'output-schedules-formatted.txt'), 'w') as f:
    f.write('\n'.join(formatted))
