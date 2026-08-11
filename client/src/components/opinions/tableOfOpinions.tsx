import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card.tsx';
import { ScrollArea } from '@/components/ui/scroll-area';
import { Collapsible, CollapsibleTrigger, CollapsibleContent } from '@/components/ui/collapsible';
import Toggle from '@/components/ui/toggle.tsx';
import type { Opinion } from '@/schemas/opinion.ts';
import OpinionLabel from '@/components/opinions/opinionLabel';

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
                      opinionId={opinion.opinionId}
                      userId={opinion.userId}
                      userName={opinion.userName}
                      content={opinion.content}
                      rating={opinion.rating}
                    />
                  </Card>
                );
              })
            )}

            {opinions.length > 2 && (
              <>
                <CollapsibleContent>
                  {opinions.slice(2).map(function (opinion) {
                    return (
                      <Card key={opinion.opinionId}>
                        <OpinionLabel
                          opinionId={opinion.opinionId}
                          userId={opinion.userId}
                          userName={opinion.userName}
                          content={opinion.content}
                          rating={opinion.rating}
                        />
                      </Card>
                    );
                  })}
                </CollapsibleContent>
                <CollapsibleTrigger asChild>
                  <Toggle OnValue="Zwiń" OffValue="Rozwiń" />
                </CollapsibleTrigger>
              </>
            )}
          </Collapsible>
        </ScrollArea>
      </CardContent>
    </Card>
  );
}
