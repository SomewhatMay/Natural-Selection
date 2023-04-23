using System.Collections.Generic;

namespace Classes;

public class BStopwatch {
    double startTime;
    private List<double> parentSamplesList;

    public BStopwatch(List<double> parentSamplesList) {
        startTime = System.Environment.TickCount;
        this.parentSamplesList = parentSamplesList;
    }

    public double Stop() {
        double difference = System.Environment.TickCount - startTime;

        parentSamplesList.Add(difference);

        return difference;
    }
}

public class Benchmark {
    private List<double> samples;
    private double currentSample;
    public bool IsStarted { get; private set; }

    private static System.InvalidOperationException alreadyStartedException = new System.InvalidOperationException("Must stop previous benchmark before calling Start() again. If you need to call Start on stack, use StartExclusive() instead.");
    private static System.InvalidOperationException notStartedException = new System.InvalidOperationException("Must call Start() before stopping again (or did you forget to use StartExclusive()?)");

    public Benchmark() {
        samples = new List<double>();
    }

    public void Start() {
        if (IsStarted) {
            throw alreadyStartedException;
        }

        IsStarted = true;
        currentSample = System.Environment.TickCount;
    }

    public double Stop() {
        if (!IsStarted) {
            throw notStartedException;
        }

        double difference = System.Environment.TickCount - currentSample;

        samples.Add(difference);

        return difference;
    }

    public double GetAverage() => GetAverage(false);
    public double GetAverage(bool clearSamples) {
        double average = 0;

        foreach (double value in samples) {
            average += value;
        }

        average /= samples.Count;

        if (clearSamples) {
            samples.Clear();
        }

        return average;
    }

    public BStopwatch StartExclusive() {
        BStopwatch watch = new BStopwatch(samples);
        return watch;
    }
}