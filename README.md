Eng | [Rus](Localization%20Readme/README_rus.md)

## Description

Program for the 2DOF moving platform. The program was created as an alternative to the official 2DOF Center program for
interaction with the platform through a third-party plugin, which must be obtained from the developers each time. Program
designed to work with the platform without outside help.

## Platform

* Windows

## How to start

* Download the archive with the program
* Unpack archive
* Run the program

## How to use

The program receives data via MemoryMapped in the form of an array of 6 elements. The first 3 elements are rotations along the axes, and
the last 3 are the speed along the axes. Data tracking occurs under the name "2DOFMemoryDataGrabber".

[Example](https://github.com/RTU-TVP/Platform-With-Steering-Wheel-SDK/blob/main/src/Platform%20With%20Steering%20Wheel%20SDK/Assets/2DOF/Sample/GameController.cs)
use in Unity.