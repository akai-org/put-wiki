import { useState, useEffect } from 'react';
import { onboardingSteps } from './OnboardingTS';
import { Button } from '@/components/ui/Button';
import {
  Card,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/Card';
import { useUserSession } from '@/features/auth';

export function Onboarding() {
  const [isOpen, setIsOpen] = useState(false);
  const [currentStep, setCurrentStep] = useState(0);
  
  const { isLoggedIn } = useUserSession(); 

  useEffect(() => {
    const hasSeenOnboarding = localStorage.getItem('hasSeenOnboarding');
    
    if (!hasSeenOnboarding && !isLoggedIn) {
      setIsOpen(true);
    }
  }, [isLoggedIn]);

  if (!isOpen) return null;

  const completeOnboarding = () => {
    localStorage.setItem('hasSeenOnboarding', 'true');
    setIsOpen(false);
  };

  const step = onboardingSteps[currentStep];
  const isLastStep = currentStep === onboardingSteps.length - 1;

  return (
    <div className="fixed inset-0 z-[100] flex overflow-y-auto bg-primary p-4 md:bg-black/50">
      
      <Card className="m-auto flex w-full max-w-[95%] sm:max-w-sm flex-col justify-between rounded-3xl border-0 bg-card shadow-2xl md:max-w-lg md:rounded-xl md:border md:shadow-none">
        
        <CardHeader className="flex-1 items-center justify-center p-6 text-center md:p-8">
          <img 
            alt={`Ilustracja kroku ${currentStep + 1}`} 
            className="mx-auto mb-6 max-h-[30vh] w-full max-w-[220px] object-contain drop-shadow-sm sm:max-w-[260px] md:max-h-[40vh] md:max-w-[320px]"
            src={step.imageSrc} 
          />
          <CardTitle className="mb-4 text-xl font-bold text-card-foreground md:text-2xl">
            {step.title}
          </CardTitle>
          <CardDescription className="flex min-h-[90px] w-full items-center justify-center rounded-xl bg-muted/50 p-4 text-sm text-muted-foreground md:text-base">
            {step.description}
          </CardDescription>
        </CardHeader>
        
        <CardFooter className="flex flex-col gap-6 pb-8 pt-4 md:pb-6">
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
                onClick={() => setCurrentStep((prev) => Math.max(0, prev - 1))} 
                variant="outline"
              >
                Wstecz
              </Button>
              <Button 
                className="shadow-none active:bg-primary/70"
                onClick={() => isLastStep ? completeOnboarding() : setCurrentStep((prev) => prev + 1)}
              >
                {isLastStep ? 'Zakończ' : 'Dalej'}
              </Button>
            </div>
          </div>

          <div className="flex justify-center gap-2">
            {onboardingSteps.map((_, index) => (
              <div 
                key={index} 
                className={`h-2 rounded-full transition-all duration-300 ${
                  index === currentStep ? 'w-6 bg-primary' : 'w-2 bg-border'
                }`}
              />
            ))}
          </div>
        </CardFooter>
      </Card>
    </div>
  );
}