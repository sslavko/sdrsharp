SignalRecorder is SDR Sharp plugin used for meteor detection by looking for forward scattered radio signals. This was developed as part of University of Auckland's student honors project. More information is available at this blog: https://www.physics.auckland.ac.nz/research/astrophysics-and-astronomy/author/anas/
User can define a threshold which triggers signal recording including one second of data before trigger and two seconds of data after last threshold crossing is detected. 
Every time signal's amplitude crosses threshold the "two seconds" timer is restarted. This way it is ensured that none of initial or trailing signal data is lost.

Data recorded is in custom designed .DAT files. These files consist of 32 bytes header followed by pairs of floating point numbers which is pure data dump received from SDRSharp.
Header structure is like this:

- Date and time when file is created. This is DateTime.ToBinary() and takes 8 bytes
- Sample rate - double - 8 bytes
- Center frequency - long - 8 bytes
- Recording frequency - long - 8 bytes

Saved files can be played back using SignalPlayer. In Source tab select Signal Player and then in file open dialog box find .dat file to play.