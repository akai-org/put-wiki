import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/Card';
import { ScrollArea } from '@/components/ui/ScrollArea';
import { Collapsible, CollapsibleTrigger, CollapsibleContent } from '@/components/ui/Collapsible';
import Toggle from '@/components/ui/Toggle';
import type { Opinion } from '@/features/opinions/opinionSchema';
import OpinionLabel from './OpinionLabel';

export default function TableOfOpinions({ opinions }: { opinions: Opinion[] }) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Opinie o kierunku</CardTitle>
      </CardHeader>
      <CardContent>
        <ScrollArea className="h-72 rounded-md border p-4">
          <Collapsible>
            {opinions.length === 0 ? (
              <p>Brak opinii o kierunku</p>
            ) : (
              opinions.slice(0, 2).map(function (opinion) {
                return (
                  <Card key={opinion.opinionId}>
                    <OpinionLabel
                      content={opinion.content}
                      opinionId={opinion.opinionId}
                      rating={opinion.rating}
                      userId={opinion.userId}
                      userName={opinion.userName}
                    />
                  </Card>
                );
              })
            )}

            {opinions.length > 2 ? (
              <>
                <CollapsibleContent>
                  {opinions.slice(2).map(function (opinion) {
                    return (
                      <Card key={opinion.opinionId}>
                        <OpinionLabel
                          content={opinion.content}
                          opinionId={opinion.opinionId}
                          rating={opinion.rating}
                          userId={opinion.userId}
                          userName={opinion.userName}
                        />
                      </Card>
                    );
                  })}
                </CollapsibleContent>
                <CollapsibleTrigger asChild>
                  <Toggle offValue="Rozwiń" onValue="Zwiń" />
                </CollapsibleTrigger>
              </>
            ) : null}
          </Collapsible>
        </ScrollArea>
      </CardContent>
    </Card>
  );
}
