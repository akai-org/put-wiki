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

export default function LoginPage() {
  return (
    <div className="flex min-h-screen items-center justify-center bg-muted/40">
      <Card className="w-full max-w-md shadow-lg !p-3">
        <CardHeader className="space-y-2">
          <CardTitle className="text-2xl font-semibold">Login to your account</CardTitle>
          <CardDescription>Enter your credentials to continue</CardDescription>
        </CardHeader>

        <CardContent className=" pb-6">
          <form className="!space-y-4">
            <div className="space-y-2">
              <label className="text-sm font-medium">Username</label>
              <Input type="text" placeholder="yourname" required />
            </div>

            <div className="space-y-2">
              <label className="text-sm font-medium">Password</label>
              <Input type="password" placeholder="••••••••" required />
            </div>

            <Button type="submit" className="w-full mt-2 bg-blue-500 hover:bg-blue-600 text-white">
              Login
            </Button>
          </form>
        </CardContent>

        <CardFooter className="justify-center border-t pt-4 text-sm text-muted-foreground">
          Forgot your password?
        </CardFooter>
      </Card>
    </div>
  );
}
