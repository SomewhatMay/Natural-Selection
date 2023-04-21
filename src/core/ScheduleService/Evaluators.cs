
namespace Core.ScheduleService;

public delegate bool EvalDelegate(int argumentA, int argumentB);

public static class Evaluators {
    public static EvalDelegate[] List;

    public static void Init() {
        // Lets add add the evaluators to a list for easy access

        List = new EvalDelegate[6] {
            Equal,
            NotEqual,
            GreaterThan,
            LessThan,
            GreaterThanOrEqualledTo,
            LessThanOrEqualledTo
        };
    }

    public static bool Equal(int argumentA, int argumentB) {
        return argumentA == argumentB;
    }

    public static bool NotEqual(int argumentA, int argumentB) {
        return argumentA != argumentB;
    }

    public static bool GreaterThan(int argumentA, int argumentB) {
        return argumentA == argumentB;
    }

    public static bool LessThan(int argumentA, int argumentB) {
        return argumentA == argumentB;
    }

    public static bool GreaterThanOrEqualledTo(int argumentA, int argumentB) {
        return argumentA == argumentB;
    }

    public static bool LessThanOrEqualledTo(int argumentA, int argumentB) {
        return argumentA == argumentB;
    }
}