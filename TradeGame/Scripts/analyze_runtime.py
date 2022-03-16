import os
import subprocess
import time
import matplotlib.pyplot as plt
import numpy as np

depthsAndFrontierLimits = { 'd1t': [1, 'true'], 'd1f': [1, 'false'], 'd3t': [3, 'true'], 'd3f': [3, 'false'], 'd5t': [5, 'true'] }
d1tTimes = []
d1fTimes = []
d3tTimes = []
d3fTimes = []
d5tTimes = []

for df in depthsAndFrontierLimits:
    print(f"Current depth/frontier config: {df}")
    for i in range(1, 11, 1):
        userDirectory = os.environ['USERPROFILE']
        exePath = os.path.join(userDirectory, "repos", "Vandy", "AI", "ldennington-5260-ai-project", 
                                        "TradeGame", "bin", "Release", "net6.0", "TradeGame.exe")
        start = time.time()
        subprocess.call(f"{exePath} --depth={depthsAndFrontierLimits[df][0]} --limit-frontier-size={depthsAndFrontierLimits[df][1]}", shell=True)
        end = time.time()

        if (df == 'd1t'):
            d1tTimes.append(end-start)

        if (df == 'd1f'):
            d1fTimes.append(end-start)

        if (df == 'd3t'):
            d3tTimes.append(end-start)

        if (df == 'd3f'):
            d3fTimes.append(end-start)
        
        if (df == 'd5t'):
            d5tTimes.append(end-start)

font = {'fontname':'Segoe UI'}

plt.xlabel('run', **font)
plt.ylabel('time (seconds)', **font)

plt.title(f'Runtimes of Various Depths with and without Beam', **font)
x = range(1, 11, 1)
plt.plot(x, d1tTimes, label = "Depth 1 with Beam", c = "#F0A22E")
plt.plot(x, d1fTimes, label = "Depth 1 without Beam", c = "#A2E8A2")
plt.plot(x, d3tTimes, label = "Depth 3 with Beam", c = "#FC05CB")
plt.plot(x, d3fTimes, label = "Depth 3 without Beam", c = "#00B0F0")
plt.plot(x, d5tTimes, label = "Depth 5 with Beam", c = "#9999FF")

plt.xticks(np.arange(min(x), max(x)+1, 1.0))
plt.legend()
plt.show()