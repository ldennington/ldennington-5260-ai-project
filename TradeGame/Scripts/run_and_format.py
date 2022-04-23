import os
import time

userProfile = os.environ['USERPROFILE']
appPath = os.path.join(userProfile, "repos", "Vandy", "AI", "ldennington-5260-ai-project", "TradeGame")

start = time.perf_counter()
os.chdir(appPath)
os.system("dotnet run")
end = time.perf_counter()

print(f"Run time: {end - start} seconds")

os.chdir(os.path.join(appPath, "Scripts"))
os.system("py format_training.py")