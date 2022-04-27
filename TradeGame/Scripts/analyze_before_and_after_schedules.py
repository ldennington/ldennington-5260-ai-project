import os
import json
import matplotlib.pyplot as plt

userProfile = os.environ['USERPROFILE']
directoryPath = os.path.join(userProfile, 'OneDrive - Vanderbilt', '5260', 'Programming Project', 'Part 2', 'Data')
frontierSizesAndDepths = [ 75, 100, 125, 5, 10, 15, 20 ]

for n in frontierSizesAndDepths:
    before = open(os.path.join(directoryPath, f'output-schedules-before-{n}.json'))
    after = open(os.path.join(directoryPath, f'output-schedules-after-{n}.json'))

    beforeSchedules = json.load(before)
    afterSchedules = json.load(after)

    beforeExpectedUtilitiesAndOrder = {}
    afterExpectedUtilitiesAndOrder = {}

    i = 0
    for eu in beforeSchedules:
        Actions = len(eu['Actions'])
        beforeExpectedUtilitiesAndOrder.update({ eu['Actions'][Actions - 1]['Expected Utility']: i })
        i += 1

    i = 0
    for eu in afterSchedules:
        Actions = len(eu['Actions'])
        afterExpectedUtilitiesAndOrder.update({ eu['Actions'][Actions - 1]['Expected Utility']: i })
        i += 1

    font = {'fontname':'Segoe UI'}

    plt.xlabel('Order', **font)
    plt.ylabel('Expected Utility', **font)

    if (n > 20):
        plt.title(f'Expected Utilities Before and After Model Implementation\nDepth: 5 Frontier Boundary: {n}', **font)
    else:
        plt.title(f'Expected Utilities Before and After Model Implementation\nDepth: {n} Frontier Boundary: 100', **font)

    x1 = beforeExpectedUtilitiesAndOrder.values()
    y1 = beforeExpectedUtilitiesAndOrder.keys()

    x2 = afterExpectedUtilitiesAndOrder.values()
    y2 = afterExpectedUtilitiesAndOrder.keys()

    plt.plot(x1, y1, c='#9999FF', label = 'Expected utilities before model implementation')
    plt.plot(x2, y2, c='#FC05CB', label = 'Expected utilities after model implementation')

    plt.legend()
    plt.show()