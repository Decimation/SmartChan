// Deci SmartChan.Lib BoardIdentifier.cs
// $File.CreatedYear-$File.CreatedMonth-22 @ 16:0

// ReSharper disable InconsistentNaming

using SmartChan.Lib.Utilities;

namespace SmartChan.Lib.Model;

/// <summary>
/// Where
/// <i><c>bi</c></i> is a <see cref="ChanBoardId"/>
/// <para />
/// <see cref="ChanHelper.BI_R">Removal</see>
/// <br />
/// <see cref="ChanHelper.BI_CMB">Combination</see>
/// <br />
/// <see cref="ChanHelper.BI_WLD">Wildcard</see>
/// <br />
/// <see cref="ChanHelper.BI_S">Special</see>
/// </summary>
[Flags]
public enum ChanBoardId : ulong
{

	#region Special

	s_None = 1UL << 0,

	#endregion

	aco    = 1UL << 1,
	adv    = 1UL << 2,
	an     = 1UL << 3,
	asp    = 1UL << 4,
	b      = 1UL << 5,
	bant   = 1UL << 6,
	biz    = 1UL << 7,
	c      = 1UL << 8,
	can    = 1UL << 9,
	cgl    = 1UL << 10,
	ck     = 1UL << 11,
	cm     = 1UL << 12,
	cock   = 1UL << 13,
	d      = 1UL << 14,
	diy    = 1UL << 15,
	e      = 1UL << 16,
	f      = 1UL << 17,
	fitlit = 1UL << 18,
	gd     = 1UL << 19,
	gif    = 1UL << 20,
	h      = 1UL << 21,
	hc     = 1UL << 22,
	his    = 1UL << 23,
	hm     = 1UL << 24,
	hr     = 1UL << 25,
	i      = 1UL << 26,
	ic     = 1UL << 27,
	lgbt   = 1UL << 28,
	lit    = 1UL << 29,
	mlpol  = 1UL << 30,
	mo     = 1UL << 31,
	mtv    = 1UL << 32,
	n      = 1UL << 33,
	news   = 1UL << 34,
	o      = 1UL << 35,
	r_out  = 1UL << 36,
	outsoc = 1UL << 37,
	p      = 1UL << 38,
	po     = 1UL << 39,
	pw     = 1UL << 40,
	q      = 1UL << 41,
	qa     = 1UL << 42,
	qst    = 1UL << 43,
	r      = 1UL << 44,
	s      = 1UL << 45,
	soc    = 1UL << 46,
	spa    = 1UL << 47,
	trv    = 1UL << 48,
	u      = 1UL << 49,
	vint   = 1UL << 50,
	vip    = 1UL << 51,
	vrpg   = 1UL << 52,
	w      = 1UL << 53,
	wg     = 1UL << 54,
	wsg    = 1UL << 55,
	wsr    = 1UL << 56,
	x      = 1UL << 57,
	y      = 1UL << 58,
	con    = 1UL << 59,
	fap    = 1UL << 60,
	t      = 1UL << 61,

	#region Combinations

	cmb_ArchiveOfSins = h | hc | hm | i | lgbt | r | s | soc | t | u,

	cmb_ArchivedMoe = aco      | adv  | an   | asp   | b | bant | biz | c | can | cgl | ck | cm | cock | d | diy | e | f
	                  | fitlit | gd   | gif  | h     | hc | his | hm | hr | i | ic | lgbt | lit | mlpol | mo | mtv
	                  | n      | news | o    | r_out | outsoc | p | po | pw | q | qa | qst | r | s | soc | spa | trv | u
	                  | vint   | vip  | vrpg | w     | wg | wsg | wsr | x | y | con | fap,

	#endregion

	#region Wildcard

	/// <summary>
	/// <see cref="ChanHelper.BI_WLD"/> → <see cref="ChanHelper.BI_WLD_PARAM"/>
	/// </summary>
	wld_Any = 1UL << -1,

	#endregion

}