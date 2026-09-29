import { Card, CardContent } from '@/components/ui/Card';
import type { BaseInfo } from '../schemas/baseInfoSchema';

export default function BaseInfoCard({ ...baseInfo }: BaseInfo) {
  return (
    <Card>
      <CardContent className="flex flex-row gap-4">
        <img
          alt="lecturer face"
          className="size-12 rounded-full object-cover md:size-20 lg:size-28"
          src={`${baseInfo.photoUrl}`}
        />

        <div className="flex flex-col text-primary-foreground">
          <p className="text-xl font-medium">{baseInfo.title}</p>
          <p className="text-3xl font-bold">{baseInfo.name}</p>
          <div className="text-sm text-muted-foreground">tag: Informatyka, Kryptografia</div>
        </div>
      </CardContent>
    </Card>
  );
}
