export default function Consultations(props: {
  date: string;
  time: string;
  interval: string;
  weekday: string;
  place: string;
  last_updated: string;
}) {
  return (
    <div className="flex flex-col gap-4 p-12 pt-4 pl-4 bg-gray-50 rounded-xl">
      <div>
        <p className="text-2xl">Konsultacje</p>
        <p>ostatnia akutalziacja: {props.last_updated}</p>
      </div>
      <div>
        {' '}
        <p className="text-xl">Gdzie?</p>
        {props.place}
      </div>
      <div>
        {' '}
        <p className="text-xl">Kiedy?</p>
        {props.interval} {props.weekday} {props.time}
      </div>
      <p className="text-center">
        Widzisz błąd / dane są nieporawne <a href="mailto:idk@putwiki.com">zgłoś to tutaj</a>
      </p>
    </div>
  );
}
