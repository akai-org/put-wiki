import {Button} from "@/components/ui/button";
import {Card,CardFooter, CardHeader, CardTitle, CardContent, CardDescription} from "@/components/ui/card";
import {Input} from "@/components/ui/input";
export default function LoginPage() {
return (
    <div className="flex h-screen items-center justify-center">
        <Card className="w-full max-w-sm">
            <CardHeader>
                <CardTitle>Login to your account</CardTitle>
                <CardDescription>Enter your credentials to login.</CardDescription>
            </CardHeader>
            <CardContent>
                <form className="flex flex-col gap-6">
                    <Input type="text" placeholder="Username" required />
                    <Input type="password" placeholder="Password" required />
                </form>
            </CardContent>
            <CardFooter className="flex-col gap-2">
                <Button type="submit" className="w-full">Login</Button>
            </CardFooter>

        </Card>
    </div>
)

}