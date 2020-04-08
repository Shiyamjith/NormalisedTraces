# NormalisedTraces

Alpha trace data collected using JYU Grain software.

## Instructions for use
1. When a new detector has been installed, run an alpha trace on it to obtain a 'good' trace.
   Run this application using the **-g** / **-good** parameter and passing the filename of the good trace.
   This will read in and store the good trace with this application.
1. At the start of a day, run an alpha trace to obtain a 'bad' (usually only slightly damaged) trace.
   Run this application using the **-b** / **-bad** parameter and passing the filename of the daily bad trace.
   This application will then create a delta file of the differences between the good and bad trace.
1. The application is now ready for use.
   Run this application using the **-n** / **-normalise** parameter and passing the filename of the trace to be normalised.
   Also specify the **-o** / **-output** parameter passing in the name of the folder to write output to.

## How does it work
Over time, detectors degrade in their performance. 
Reading a good alpha trace when the detector is quite new allows us to calculate 
how much the detector is damaged and perform normalisation of traces.

For good alpha or bad alpha trace files, all the data is read in from the
traces given to it.
Each column (time period) of the data is sorted into numerical order.
The top 25% and bottom 25% of the data is ignored to remove anomalies.
The centre 50% of the data is averaged to obtain a decent signal value for the trace.
For both good and bad alpha traces, this data is simply stored in binary files
with the application.

For normalising standard/non-alpha traces, use the -n and -o parameters.
The delta file is read in and stored in memory, and each value in the standard
trace file is multiplied by the relevant columns delta value to normalise the
data back to the same state as if the detector was new.

## Command line
```
NormaliseTrace <command> [filename]
```

### Available commands:

-g   **Good** 
     Reads in a good alpha trace file and stores it as the new reference trace.
     Use this only when the detector is quite new.

-b   **Bad** 
     Reads in a bad alpha trace file and calculates a delta file to use to normalise traces.
     Ideally do this every day, or at least once a week.

-n   **Normalise**
     The trace file to be normalised. Also specify the -o parameter.

-o   **Output folder**
     The output folder to write the normalised traces. If an output file already exists, it will be overwritten.

Examples:
```
NormaliseTrace -g c:\\traces\\good\\R3_Strip*.csv
NormaliseTrace -b c:\\traces\\bad\\R3_Strip*.csv
NormaliseTrace -n c:\\traces\\March07\\Sham*.csv -o c:\\traces\\March07\\Output
```