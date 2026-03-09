# Slot Casino Game

## Overview
Unity project that simulates a slot machine game. The game features a basic user interface where players can spin the reels and win or lose based on the outcome.

It uses a simple client server architecture, where the client is responsible for the user interface and sending requests to the server, while the server handles the game logic and determines the outcome of each spin.

## Features
- Simple and intuitive user interface for spinning the reels.
- Basic game logic to determine wins and losses based on the symbols that appear on the reels.
- Communication between the client and server to process game actions.
- Randomized outcomes for each spin to ensure a fair gaming experience. If you want to create more bet windows, check Server / Constants.cs file > MaxBetWindows.
- Interfaces to allow winlines custom animations. Check IWinLineAnimation.cs file for more details.

([https://raw.githubusercontent.com/username/repository/branch/path/to/video.mp4](https://github.com/salvadorlemus/Unity-Slot-Casino-Game/blob/main/SlotCasinoShort.mp4))

## Getting Started
1. Clone the repository to your local machine.
2. Open the project in Unity 6.3 LTS prefered.
3. Open the `SampleScene` to see the game in action.
4. Press the "Play" button or the space bar to start spinning the reels and see if you win or lose.

## Know bugs
- Symbols can be cut by the reel borders over the time.

## Future Improvements
- Add RTP (Return to Player) system to control the payout percentage of the game.
- Implement a more complex betting system with multiple bet levels and paylines.
- Enhance the user interface with animations and sound effects for a more engaging gaming experience.

## License
This project is licensed under the MIT License.
