import os
import subprocess
import time
import matplotlib.pyplot as plt
import statistics
import numpy as np

depthsAndFrontierLimits = { 'd5f75': [5, 75], 'd10f75': [10, 75], 'd15f75': [15, 75], 'd20f75': [20, 75], \
    'd5f100': [5, 100], 'd10f100': [10, 100], 'd15f100': [15, 100], 'd20f100': [20, 100], \
    'd5f125': [5, 125], 'd10f125': [10, 125], 'd15f125': [15, 125], 'd20f125': [20, 125] }

d5f75Times = []
d10f75Times = []
d15f75Times = []
d20f75Times = []
d5f100Times = []
d10f100Times = []
d15f100Times = []
d20f100Times = []
d5f125Times = []
d10f125Times = []
d15f125Times = []
d20f125Times = []


for df in depthsAndFrontierLimits:
    print(f"Current depth/frontier config: {df}")
    for i in range(1, 11, 1):
        userDirectory = os.environ['USERPROFILE']
        exePath = os.path.join(userDirectory, "repos", "Vandy", "AI", "ldennington-5260-ai-project", 
                                        "TradeGame", "bin", "Release", "net6.0", "TradeGame.exe")
        start = time.time()
        subprocess.call(f"{exePath} --depth={depthsAndFrontierLimits[df][0]} --frontier-boundary={depthsAndFrontierLimits[df][1]}", shell=True)
        end = time.time()

        if (df == 'd5f75'):
            d5f75Times.append(end-start)

        if (df == 'd10f75'):
            d10f75Times.append(end-start)

        if (df == 'd15f75'):
            d15f75Times.append(end-start)

        if (df == 'd20f75'):
            d20f75Times.append(end-start)
        
        if (df == 'd5f100'):
            d5f100Times.append(end-start)

        if (df == 'd10f100'):
            d10f100Times.append(end-start)

        if (df == 'd15f100'):
            d15f100Times.append(end-start)

        if (df == 'd20f100'):
            d20f100Times.append(end-start)

        if (df == 'd5f125'):
            d5f125Times.append(end-start)

        if (df == 'd10f125'):
            d10f125Times.append(end-start)

        if (df == 'd15f125'):
            d15f125Times.append(end-start)

        if (df == 'd20f125'):
            d20f125Times.append(end-start)

# Plot runtimes
font = {'fontname':'Segoe UI'}

plt.xlabel('run', **font)
plt.ylabel('time (seconds)', **font)

plt.title(f'Runtimes of Various Depths and Frontier Boundaries', **font)
x = range(1, 11, 1)
plt.plot(x, d5f75Times, label = "Depth 5 Frontier 75", c = "#F0A22E")
plt.plot(x, d10f75Times, label = "Depth 10 Frontier 75", c = "#A2E8A2")
plt.plot(x, d15f75Times, label = "Depth 15 Frontier 75", c = "#FC05CB")
plt.plot(x, d20f75Times, label = "Depth 20 Frontier 75", c = "#00B0F0")
plt.plot(x, d5f100Times, label = "Depth 5 Frontier 100", c = "#9999FF")
plt.plot(x, d10f100Times, label = "Depth 10 Frontier 100", c = "#7F7F7F")
plt.plot(x, d15f100Times, label = "Depth 15 Frontier 100", c = "#000000")
plt.plot(x, d20f100Times, label = "Depth 20 Frontier 100", c = "#FFB6C1")
plt.plot(x, d5f125Times, label = "Depth 5 Frontier 125", c = "#50C878")
plt.plot(x, d10f125Times, label = "Depth 10 Frontier 125", c = "#301934")
plt.plot(x, d15f125Times, label = "Depth 15 Frontier 125", c = "#FEDD00")
plt.plot(x, d20f125Times, label = "Depth 20 Frontier 125", c = "#FF7F50")

plt.xticks(np.arange(min(x), max(x)+1, 1.0))
plt.legend(loc='center left', bbox_to_anchor=(1, 0.5))
plt.show()

# Clear plot
plt.clf()

d5f75StdDev = statistics.stdev(d5f75Times)
d10f75StdDev = statistics.stdev(d10f75Times)
d15f75StdDev = statistics.stdev(d15f75Times)
d20f75StdDev = statistics.stdev(d20f75Times)
d5f100StdDev = statistics.stdev(d5f100Times)
d10f100StdDev = statistics.stdev(d10f100Times)
d15f100StdDev = statistics.stdev(d15f100Times)
d20f100StdDev = statistics.stdev(d20f100Times)
d5f125StdDev = statistics.stdev(d5f125Times)
d10f125StdDev = statistics.stdev(d10f125Times)
d15f125StdDev = statistics.stdev(d15f125Times)
d20f125StdDev = statistics.stdev(d20f125Times)

standardDeviations = { 'd5f75': d5f75StdDev, 'd10f75': d10f75StdDev, 'd15f75': d15f75StdDev, 'd20f75': d20f75StdDev, \
    'd5f100': d5f100StdDev, 'd10f100': d10f100StdDev, 'd15f100': d15f100StdDev, 'd20f100': d20f100StdDev, \
    'd5f125': d5f125StdDev, 'd10f125': d10f125StdDev, 'd15f125': d15f125StdDev, 'd20f125': d20f125StdDev }

# Plot standard deviation of runtimes
plt.xlabel('run', **font)
plt.ylabel('standard deviation', **font)

plt.title(f'Standard Deviations of Various Depths and Frontier Boundaries', **font)

plt.bar(standardDeviations.keys(), standardDeviations.values(), color = 'pink', width = 0.3)
plt.show()