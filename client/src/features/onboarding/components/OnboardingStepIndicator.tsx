interface OnboardingStepIndicatorProps {
  currentStep: number;
  totalSteps: number;
}

export function OnboardingStepIndicator({ currentStep, totalSteps }: OnboardingStepIndicatorProps) {
  return (
    <div className="flex justify-center gap-2">
      {Array.from({ length: totalSteps }).map((_, index) => (
        <div
          className={`h-2 rounded-full transition-all duration-300 ${
            index === currentStep ? 'w-6 bg-primary' : 'w-2 bg-border'
          }`}
          key={index}
        />
      ))}
    </div>
  );
}
