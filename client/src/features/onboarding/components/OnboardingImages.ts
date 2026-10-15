import cat1 from '@/assets/graphics/onboarding/Hallo.webp';
import cat2 from '@/assets/graphics/onboarding/Info.webp';
import cat3 from '@/assets/graphics/onboarding/Etyka.webp';
import cat4 from '@/assets/graphics/onboarding/HowWork.webp';
import cat5 from '@/assets/graphics/onboarding/Anonim.webp';
import cat6 from '@/assets/graphics/onboarding/USOS.webp';

// 1. Dodajemy imageAlt do interfejsu
export interface OnboardingStep {
  id: number;
  title: string;
  description: string;
  imageSrc: string;
  imageAlt: string;
}

export const onboardingSteps: OnboardingStep[] = [
  {
    id: 1,
    title: 'Witaj na naszej stronie!',
    description: '',
    imageSrc: cat1,
    imageAlt: 'Wesoły kot machający łapką na powitanie',
  },
  {
    id: 2,
    title: 'Wyszukuj informacje o przedmiotach i prowadzących',
    description: '',
    imageSrc: cat2,
    imageAlt: 'Kot z lupą przeszukujący dokumenty',
  },
  {
    id: 3,
    title: 'Pamiętaj o kulturze osobistej!',
    description: '',
    imageSrc: cat3,
    imageAlt: 'Uprzejmy, elegancki kot przypominający o dobrych manierach',
  },
  {
    id: 4,
    title: 'Jak działa strona?',
    description: '',
    imageSrc: cat4,
    imageAlt: 'Zastanawiający się kot patrzący na schemat działania',
  },
  {
    id: 5,
    title: 'Jesteś całkowicie anonimowy',
    description: '',
    imageSrc: cat5,
    imageAlt: 'Kot w okularach przeciwsłonecznych i kapeluszu detektywa ukrywający swoją tożsamość',
  },
  {
    id: 6,
    title: 'Możesz zalogować się poprzez serwis USOS',
    description: '',
    imageSrc: cat6,
    imageAlt: 'Kot zadowolony z udanego logowania przez system USOS',
  },
];
