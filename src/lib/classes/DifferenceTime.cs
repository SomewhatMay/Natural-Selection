using Microsoft.Xna.Framework;
using System;

namespace Classes;

public class DifferenceTime {
    private double startTime;

    public void Start() {
        startTime = System.Environment.TickCount;
    }

    public double Calculate(bool? inSeconds = false) {
        double difference = System.Environment.TickCount - startTime;

        if (inSeconds == true) {
            difference /= 1000;
        }

        // lets round off to teh nearest hundredth
        difference = Math.Floor(difference * 100) / 100;

        return difference;
    }
}