import os
import json
import matplotlib.pyplot as plt

tempDirectory = os.environ['TEMP']

f = open(os.path.join(tempDirectory, 'output-schedules.json'))
schedules = json.load(f)

expectedUtilitiesAndOrder = {}
i = 0
for eu in schedules:
    Actions = len(eu['Actions'])
    expectedUtilitiesAndOrder.update({ eu['Actions'][Actions - 1]['Expected Utility']: i })
    i += 1

font = {'fontname':'Segoe UI'}

plt.xlabel('Order', **font)
plt.ylabel('Expected Utility', **font)

plt.title(f'Ordered Expected Utilities for {i} Schedules', **font)
x = expectedUtilitiesAndOrder.values()
y = expectedUtilitiesAndOrder.keys()

plt.scatter(x, y, c="#FC05CB")
plt.show()