import os
import time

tempDirectory = os.environ['TEMP']
pathToRuntimes = os.path.join(tempDirectory, 'runtimes.txt')
userProfile = os.environ['USERPROFILE']
appPath = os.path.join(userProfile, "repos", "Vandy", "AI", "ldennington-5260-ai-project", "TradeGame")

depths = [ 5, 10, 15, 20 ]
frontierBoundaries = [ 5, 10, 15 ]

for f in frontierBoundaries:
    for d in depths:
        print(f"Running with depth {d} and frontier bound {f}")
        start = time.perf_counter()
        os.chdir(appPath)
        os.system(f"dotnet run --depth={d} --frontier-boundary={f}")
        end = time.perf_counter()

        runtimeFile = open(pathToRuntimes, 'a', encoding='UTF8', newline='')
        runtimeFile.write(f'{d}\t{f}\t{end - start}\n')
        runtimeFile.close()

        os.chdir(os.path.join(appPath, "Scripts"))
        os.system("py format_training.py")