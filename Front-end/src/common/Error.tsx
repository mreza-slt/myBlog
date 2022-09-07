export default function Error({ error }: any) {
  return (
    <div className="mt-4">
      {error &&
        Object.values(error).map((value: any) => (
          <div key={value}>
            <span className="text-red-600">{value}</span>
            <br />
          </div>
        ))}
    </div>
  );
}
