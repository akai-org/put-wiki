import { useState, useEffect } from 'react';

const ONBOARDING_STORAGE_KEY = 'hasSeenOnboarding';

export function useOnboarding(totalSteps: number) {
  const [isOpen, setIsOpen] = useState(false);
  const [currentStep, setCurrentStep] = useState(0);

  useEffect(() => {
    const hasSeenOnboarding = localStorage.getItem(ONBOARDING_STORAGE_KEY);

    if (hasSeenOnboarding !== 'true') {
      const timer = setTimeout(() => {
        setIsOpen(true);
      }, 150);

      return () => clearTimeout(timer);
    }
  }, []);

  const completeOnboarding = () => {
    localStorage.setItem(ONBOARDING_STORAGE_KEY, 'true');
    setIsOpen(false);
  };

  const handleOpenChange = (open: boolean) => {
    setIsOpen(open);
    if (!open) {
      localStorage.setItem(ONBOARDING_STORAGE_KEY, 'true');
    }
  };

  const goToNextStep = () => {
    if (currentStep >= totalSteps - 1) {
      completeOnboarding();
    } else {
      setCurrentStep((prev) => prev + 1);
    }
  };

  const goToPrevStep = () => {
    setCurrentStep((prev) => Math.max(0, prev - 1));
  };

  return {
    isOpen,
    currentStep,
    isLastStep: currentStep === totalSteps - 1,
    completeOnboarding,
    handleOpenChange,
    goToNextStep,
    goToPrevStep,
  };
}
