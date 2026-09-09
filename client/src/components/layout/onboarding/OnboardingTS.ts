import cat1 from '@/assets/graphics/onboarding/HalloSmall.png';
import cat2 from '@/assets/graphics/onboarding/InfoSmall.png';
import cat3 from '@/assets/graphics/onboarding/EtykaSmall.png';
import cat4 from '@/assets/graphics/onboarding/HowWorkSmall.png';
import cat5 from '@/assets/graphics/onboarding/AnonimSmall.png';
import cat6 from '@/assets/graphics/onboarding/USOSSmall.png';

export interface OnboardingStep {
  id: number;
  title: string;
  description: string;
  imageSrc: string; 
}

export const onboardingSteps: OnboardingStep[] = [
  {
    id: 1,
    title: 'Witaj na naszej stronie!',
    description: '',
    imageSrc: cat1,
  },
  {
    id: 2,
    title: 'Wyszukuj informacje o przedmiotach i prowadzących',
    description: '',
    imageSrc: cat2,
  },
  {
    id: 3,
    title: 'Pamiętaj o kulturze osobistej!',
    description: '',
    imageSrc: cat3,
  },
  {
    id: 4,
    title: 'Jak działa strona?',
    description: '',
    imageSrc: cat4,
  },
  {
    id: 5,
    title: 'Jesteś całkowicie anonimowy!',
    description: '',
    imageSrc: cat5,
  },
  {
    id: 6,
    title: 'Możesz zalogować się poprzez serwis USOS',
    description: '',
    imageSrc: cat6,
  },
];