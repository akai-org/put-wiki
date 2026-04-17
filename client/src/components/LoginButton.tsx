import { useState } from 'react';
import { useNavigate } from '@tanstack/react-router';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardFooter, CardHeader, CardTitle } from '@/components/ui/card';

export default function LoginButton() {
  const [showPopup, setShowPopup] = useState(false);
  const navigate = useNavigate();

  const handleLoginClick = () => {
    setShowPopup(true);
  };

  const handleConfirm = () => {
    setShowPopup(false);
    
  };

  const handleCancel = () => {
    setShowPopup(false);
  };

  return (
    <>
      <Button
        onClick={handleLoginClick}
        className="bg-blue-500 hover:bg-blue-600 text-white"
      >
        Login
      </Button>

      {showPopup && (
        <div className="fixed inset-0 flex items-center justify-center bg-black/50 z-50">
          <Card className="w-full max-w-sm shadow-lg">
            <CardHeader>
              <CardTitle>Data Collection Notice</CardTitle>
            </CardHeader>
            <CardContent>
              <p className="text-sm text-muted-foreground">
                We collect your data
              </p>
            </CardContent>
            <CardFooter className="flex gap-2 justify-end">
              <Button
                variant="outline"
                onClick={handleCancel}
              >
                Cancel
              </Button>
              <Button
                onClick={handleConfirm}
                className="bg-blue-500 hover:bg-blue-600 text-white"
              >
                Confirm
              </Button>
            </CardFooter>
          </Card>
        </div>
      )}
    </>
  );
}
