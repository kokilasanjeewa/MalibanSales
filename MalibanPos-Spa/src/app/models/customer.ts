export class Customer {
  constructor(
    public number: string,
    public fullName: string,
    public creditLimit: number,
    public cashLimit: number,
    public previousCreditBalance: number,
    public creditBalance: number,
    public cashBalance: number,

    public epfNo: string,
  ) { }
}

