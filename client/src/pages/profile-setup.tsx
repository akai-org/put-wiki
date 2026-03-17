import { useState } from 'react';
import { Button } from '@/components/ui/button';
import {
  Card,
  CardFooter,
  CardHeader,
  CardTitle,
  CardContent,
  CardDescription,
} from '@/components/ui/card';
import { Input } from '@/components/ui/input';

export default function ProfileSetupPage() {
  const [nickname, setNickname] = useState('');

  const handleConfirm = () => {
    console.log('Profile setup confirmed with nickname:', nickname);
    // TODO
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-muted/40">
      <Card className="w-full max-w-md shadow-lg !p-3">
        <CardHeader className="space-y-2">
          <CardTitle className="text-2xl font-semibold">Set up your profile</CardTitle>
          <CardDescription>Choose a nickname to get started</CardDescription>
        </CardHeader>

        <CardContent className="pb-6">
          <form className="!space-y-4" onSubmit={(e) => { e.preventDefault(); handleConfirm(); }}>
            <div className="space-y-2">
              <label className="text-sm font-medium">Nickname</label>
              <Input
                type="text"
                placeholder="Enter your nickname"
                value={nickname}
                onChange={(e) => setNickname(e.target.value)}
                required
              />
            </div>

            <Button
              type="submit"
              disabled={!nickname.trim()}
              className="w-full mt-2 bg-blue-500 hover:bg-blue-600 text-white disabled:opacity-50 disabled:cursor-not-allowed"
            >
              Confirm
            </Button>
          </form>
        </CardContent>
      </Card>
    </div>
  );
}
