# Terra

## Background

Welcome to the world of Terra! This world consists of 5 different countries, all named after Earth gods/goddesses from various world mythologies:

| Name       | Origin                  |
|------------|-------------------------|
| Gaia       | Greek mythology         |
| Houtu      | Chinese folk religion   |
| Mat Zemlya | Slavic                  |
| Pachamama  | Andean (Incan, Aymaran) |
| Sif        | Norse mythology         |

Source: https://en.wikipedia.org/wiki/Earth_goddess

Each of these countries deeply respects the environment and has a goal of minimizing its ecological footprint.

## Working with the project

__Note:__ The below steps have only been validated on a Windows machine. Although the code _may_ run on other operating systems with .NET installed, this is not guaranteed.

### Viewing the code

1. Install [Visual Studio Code](https://code.visualstudio.com/download), ensuring you choose to add to your `%PATH%`.
0. Open a Command Prompt.
0. Clone this repository.
    ```cmd
    git clone https://github.com/ldennington/ldennington-5260-ai-project.git
    ```
0. Navigate to the top-level directory.
    ```cmd
    cd ldennington-5260-ai-project
    ```
0. Open with Visual Studio Code.
    ```cmd
    code .
    ```
0. The entry point for the program is the `GameScheduler()` method in `ScheduleGenerator.cs` (called by `scheduleGenerator.GameScheduler()` in `Program.cs` on startup).

### Running the code

1. Install the [.NET 6.0 sdk](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.201-windows-x64-installer).
0. Open a Command Prompt.
0. If you have not already done so, clone this repository.
    ```cmd
    git clone https://github.com/ldennington/ldennington-5260-ai-project.git
    ```
0. Navigate to the `TradeGame` directory.
    ```cmd
    cd ldennington-5260-ai-project\TradeGame
    ```
0.  Run the project.
    ```
    dotnet run
    ```
0.  Open the output json file.
    ```
    code %TEMP%/output-schedules.json
    ```

### Validating the code
1. If you have not already done so, install the [.NET 6.0 sdk](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.201-windows-x64-installer).
0. Open a Command Prompt.
0. If you have not already done so, clone this repository.
    ```cmd
    git clone https://github.com/ldennington/ldennington-5260-ai-project.git
    ```
0. Navigate to the `TradeGame.Test` directory.
    ```cmd
    cd ldennington-5260-ai-project\TradeGame.Test
    ```
0. Run the project unit tests.
    ```
    dotnet test
    ```

Note that the project also contains a special [GitHub Actions workflow](.github/workflows/ci.yml) that runs `dotnet build` and `dotnet test` on every Pull Request and new commit in `main` to protect against regressions.
