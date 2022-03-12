import os
import json
import matplotlib.pyplot as plt
import pandas as pd

temp_directory = os.environ['TEMP']

f = open(os.path.join(temp_directory, 'output-schedules.json'))
schedules = json.load(f)

expectedUtilitiesAndOrder = {}
i = 0
for eu in schedules:
    expectedUtilitiesAndOrder.update({ eu['ExpectedUtility']: i })
    i += 1

plt.xlabel('order')
plt.ylabel('expected utility')

plt.title(f'Ordered Expected Utilities for {i} Schedules')
x = expectedUtilitiesAndOrder.values()
y = expectedUtilitiesAndOrder.keys()

plt.scatter(x, y)
plt.show()