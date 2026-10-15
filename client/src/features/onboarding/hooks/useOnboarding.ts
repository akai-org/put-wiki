import { useState, useEffect } from 'react';
import { useUserSession } from '@/features/auth';

const ONBOARDING_STORAGE_KEY = 'hasSeenOnboarding';

export function useOnboarding(totalSteps: number) {
  const [isOpen, setIsOpen] = useState(false);
  const [currentStep, setCurrentStep] = useState(0);
  const { isLoggedIn } = useUserSession();

  useEffect(() => {
    const hasSeenOnboarding = localStorage.getItem(ONBOARDING_STORAGE_KEY);

    const timer = setTimeout(() => {
      if (!isLoggedIn) {
        setIsOpen(true);
      }
    }, 150);

    if (hasSeenOnboarding === 'true') {
      return;
    }

    return () => clearTimeout(timer);
  }, [isLoggedIn]);

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
