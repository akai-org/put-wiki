import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card.tsx';
import { ScrollArea } from '@/components/ui/scroll-area';
import { Collapsible, CollapsibleTrigger, CollapsibleContent } from '@/components/ui/collapsible';
import Toggle from '@/components/ui/toggle.tsx';
import Opinion from '@/components/opinions/opinion.tsx';

interface Opinion {
  author: string;
  content: string;
  rating: number;
}

export default function TableOfOpinions({ opinions }: { opinions: Opinion[] }) {
  return (
    <Card>
      <CardHeader>
        <CardTitle>Opinie o kierunku</CardTitle>
      </CardHeader>
      <CardContent>
        <ScrollArea className="h-72 rounded-md border p-4">
          <Collapsible>
            <Card className="przykladowa-opinia">
              {opinions.length > 0 ? (
                opinions.slice(0, 2).map(function (opinion, index) {
                  return (
                    <Card key={index}>
                      <Opinion
                        author={opinion.author}
                        content={opinion.content}
                        rating={opinion.rating}
                      />
                    </Card>
                  );
                })
              ) : (
                <CardContent>
                  <p>Brak opinii</p>
                </CardContent>
              )}
            </Card>
            <CollapsibleContent>
              {opinions.length > 0 ? (
                opinions.slice(2).map(function (opinion, index) {
                  return (
                    <Card key={index}>
                      <Opinion
                        author={opinion.author}
                        content={opinion.content}
                        rating={opinion.rating}
                      />
                    </Card>
                  );
                })
              ) : (
                <p>Brak opinii</p>
              )}
            </CollapsibleContent>
            <CollapsibleTrigger asChild>
              <Toggle OnValue="Zwiń" OffValue="Rozwiń" />
            </CollapsibleTrigger>
          </Collapsible>
        </ScrollArea>
      </CardContent>
    </Card>
  );
}
