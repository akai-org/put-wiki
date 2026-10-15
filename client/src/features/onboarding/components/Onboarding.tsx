import { onboardingSteps } from './OnboardingImages';
import { useOnboarding } from '../hooks/useOnboarding';
import { OnboardingStepIndicator } from './OnboardingStepIndicator';
import { Button } from '@/components/ui/Button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from '@/components/ui/Dialog';

export function Onboarding() {
  const {
    isOpen,
    currentStep,
    isLastStep,
    completeOnboarding,
    handleOpenChange,
    goToNextStep,
    goToPrevStep,
  } = useOnboarding(onboardingSteps.length);

  const step = onboardingSteps[currentStep];

  return (
    <Dialog onOpenChange={handleOpenChange} open={isOpen}>
      <DialogContent
        className="flex w-full max-w-[95%] flex-col justify-between rounded-3xl border-0 bg-card p-6 shadow-2xl sm:max-w-sm md:max-w-lg md:rounded-xl md:border md:p-8 md:shadow-none"
        onEscapeKeyDown={(e) => e.preventDefault()}
        onInteractOutside={(e) => e.preventDefault()}
      >
        <DialogHeader className="flex-1 items-center justify-center text-center">
          <div className="mb-6 flex h-48 w-full items-center justify-center md:h-64">
            <img
              alt={step.imageAlt}
              className="max-h-full max-w-full object-contain drop-shadow-sm"
              src={step.imageSrc}
            />
          </div>

          <DialogTitle className="mb-4 text-xl font-bold text-card-foreground md:text-2xl">
            {step.title}
          </DialogTitle>

          {step.description ? (
            <DialogDescription className="flex h-24 w-full items-center justify-center rounded-xl bg-muted/50 p-4 text-sm text-muted-foreground md:h-28 md:text-base">
              {step.description}
            </DialogDescription>
          ) : (
            <DialogDescription className="sr-only">
              {step.title}
            </DialogDescription>
          )}
        </DialogHeader>

        <DialogFooter className="mt-4 flex flex-col gap-6 sm:flex-col">
          <div className="flex w-full items-center justify-between">
            <Button
              className="shadow-none active:bg-transparent"
              onClick={completeOnboarding}
              variant="ghost"
            >
              Pomiń
            </Button>

            <div className="flex gap-2">
              <Button
                className={`shadow-none active:bg-accent/70 ${currentStep === 0 ? 'invisible' : ''}`}
                onClick={goToPrevStep}
                variant="outline"
              >
                Wstecz
              </Button>
              <Button className="shadow-none active:bg-primary/70" onClick={goToNextStep}>
                {isLastStep ? 'Zakończ' : 'Dalej'}
              </Button>
            </div>
          </div>

          <OnboardingStepIndicator currentStep={currentStep} totalSteps={onboardingSteps.length} />
        </DialogFooter>
      </DialogContent>
    </Dialog>
  );
}
