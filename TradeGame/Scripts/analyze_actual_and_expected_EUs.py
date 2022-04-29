import os
import matplotlib.pyplot as plt
import pandas as pd
import sys

tempDirectory = os.environ['TEMP']

df = pd.read_csv(os.path.join(tempDirectory, 'predicted-and-actual.csv'))
predicted = df['Predicted'].tolist()
actual = df['Actual'].tolist()

font = {'fontname':'Segoe UI'}

plt.xlabel('Order', **font)
plt.ylabel('Expected Utility', **font)

plt.title(f'Predicted and Actual Expected Utilities for Test Set\nDepth: {sys.argv[1]} Frontier Boundary: {sys.argv[2]}', **font)
x = df.index.values.tolist()

plt.plot(x, predicted, label = 'predicted', c = '#A2E8A2')
plt.plot(x, actual, label = 'actual', c = '#9999FF')

plt.legend(loc='center left', bbox_to_anchor=(1, 0.5))
plt.show()