import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: 'filterUnique',
  pure: false
})
export class FilterUniquePipe implements PipeTransform {

  transform(value: any, args?: any): any {

    // Remove the duplicate elements
    // let uniqueArray = value.filter(function (el, index, array) {
    //   return array.indexOf(el) == index;
    // });

    let uniqueArray = Array.from(new Set(value.map(item => item.dSalePrice)));
    return uniqueArray;
  }

}
